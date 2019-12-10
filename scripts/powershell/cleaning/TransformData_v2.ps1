param(
    $csvPath,
    $trainingPercent = 0.6,
    $display = 2
)

Write-Host "$(Get-Date): Reading file" -ForegroundColor Yellow
Write-Progress -Activity "Reading" -Id 1 -Status "Progress.." -PercentComplete 0
$dataInCsv = Get-Content -Path $csvPath | ConvertFrom-Csv
Write-Progress -Activity "Reading" -Id 1 -Status "Progress.." -PercentComplete 100
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow

function getval($v){if($v -eq "Y" -or $v -eq "y"){return 1}elseif($v -eq "N" -or $v -eq "n"){return 0}else{return -1}}

$props = @("TOTAL_WORKERS","NEW_EMPLOYMENT","CONTINUED_EMPLOYMENT","CHANGE_PREVIOUS_EMPLOYMENT","NEW_CONCURRENT_EMPLOYMENT","CHANGE_EMPLOYER","AMENDED_PETITION","Outcome","FULL_TIME_POSITION_Num","H1B_DEPENDENT_Num","WILLFUL_VIOLATOR_Num","AGENT_REPRESENTING_EMPLOYER_Num")

function GetValid($row)
{
    if ($row.VISA_CLASS -eq "H-1B")
    {
        if ($row.CASE_STATUS -match "DENIED")
        {
            $row | Add-Member NoteProperty "Outcome" 0 -Force
        }
        elseif($row.CASE_STATUS -match "CERTIFIED")
        {
            $row | Add-Member NoteProperty "Outcome" 1 -Force
        }
        else
        {
            $row | Add-Member NoteProperty "Reason" "Visa_status" -Force
            return $null
        }

        $row | Add-Member "FULL_TIME_POSITION_Num" $(getval $row.FULL_TIME_POSITION) -Force
        $row | Add-Member "H1B_DEPENDENT_Num" $(getval $row.H1B_DEPENDENT) -Force
        $row | Add-Member "WILLFUL_VIOLATOR_Num" $(getval $row.WILLFUL_VIOLATOR) -Force
        $row | Add-Member "AGENT_REPRESENTING_EMPLOYER_Num" $(getval $row.AGENT_REPRESENTING_EMPLOYER) -Force
    
        if ($row.FULL_TIME_POSITION_Num -eq -1)
        {
            $row | Add-Member NoteProperty "Reason" "FULL_TIME_POSITION_Num Y/N" -Force
            return $null
        }
        elseif($row.H1B_DEPENDENT_Num -eq -1)
        {
            $row | Add-Member NoteProperty "Reason" "H1B_DEPENDENT_Num Y/N" -Force
            return $null
        }
        elseif($row.WILLFUL_VIOLATOR_Num -eq -1)
        {
            $row | Add-Member NoteProperty "Reason" "WILLFUL_VIOLATOR_Num Y/N" -Force
            return $null
        }
        elseif($row.AGENT_REPRESENTING_EMPLOYER_Num -eq -1)
        {
            $row | Add-Member NoteProperty "Reason" "AGENT_REPRESENTING_EMPLOYER_Num Y/N" -Force
            return $null
        }

        $res = $row | select TOTAL_WORKERS,NEW_EMPLOYMENT,CONTINUED_EMPLOYMENT,CHANGE_PREVIOUS_EMPLOYMENT,NEW_CONCURRENT_EMPLOYMENT,CHANGE_EMPLOYER,AMENDED_PETITION,Outcome,FULL_TIME_POSITION_Num,H1B_DEPENDENT_Num,WILLFUL_VIOLATOR_Num,AGENT_REPRESENTING_EMPLOYER_Num
        
        $valid = $true
        $props | %{
            $v=0
            $r = [int]::TryParse($res.$_,[ref]$v)
            if($r -eq $true)
            {
                $res.$_=$v
            }
            else
            {
                $row.$_=-1
                $row | Add-Member NoteProperty "Reason" "Prop $_" -Force
                $valid=$false
            } 
        }
        
        if($valid -eq $false)
        {
            $row | Add-Member NoteProperty "Reason" "Prop unknown" -Force
            return $null
        }

        return $res
    }

    $row | Add-Member NoteProperty "Reason" "visa class" -Force
    return $null
}

Write-Host "$(Get-Date): Transforming" -ForegroundColor Yellow
Write-Progress -Activity "Transforming" -Id 2 -Status "Progress.." -PercentComplete 0
$lim = New-Object Collections.Generic.List[psobject]
$invalid = New-Object Collections.Generic.List[psobject]
$total = $dataInCsv.Count
$i = 0
$dataInCsv | %{
    $val = GetValid $_
    if ($val -eq $null)
    {
        $invalid.Add($_)
    }
    else
    {
        $lim.Add($val)
    }
    $i++

    if ($i%1000 -eq 4)
    {
        Write-Progress -Activity "Calculating" -Id 2 -Status "Progress.." -PercentComplete (($i/$total) * 100)
    }
}
$lim = $lim.ToArray()
$invalid = $invalid.ToArray()
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow

Write-Progress -Activity "Saving" -Id 3 -Status "Progress.." -PercentComplete 0
Write-Host "$(Get-Date): Separating training and validation sets and saving as csv" -ForegroundColor Yellow
$70 = [int]($lim.count*$trainingPercent)
$lim[0..$70] | Export-Csv -Path "./training_v2_$trainingPercent.csv" -Encoding UTF8 -NoTypeInformation
$lim[$($70+1)..$($lim.count)] | Export-Csv -Path "./validation_v2_$(1-$trainingPercent).csv" -Encoding UTF8 -NoTypeInformation

$lim | Export-Csv -Path "./transformed_all_v2_$trainingPercent.csv" -Encoding UTF8 -NoTypeInformation
$invalid | Export-Csv -Path "./invalid_v2_$trainingPercent.csv" -Encoding UTF8 -NoTypeInformation
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
Write-Progress -Activity "Saving" -Id 3 -Status "Progress.." -PercentComplete 100


return $dataInCsv