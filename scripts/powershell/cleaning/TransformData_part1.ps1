param(
    $csvPath,
    $trainingPercent = 0.6,
    $display = 2
)
Write-Host "$(Get-Date): Reading file" -ForegroundColor Yellow
$dataInCsv = Get-Content -Path $csvPath | ConvertFrom-Csv
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
$dataInCsv[0..$display] | ft

Write-Host "$(Get-Date): Selecting H-1B visas" -ForegroundColor Yellow
$dataInCsv = $dataInCsv | ?{$_.VISA_CLASS -eq "H-1B"}
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
$dataInCsv[0..$display] | ft

Write-Host "$(Get-Date): Selecting coulmns CASE_STATUS,TOTAL_WORKERS,NEW_EMPLOYMENT,CONTINUED_EMPLOYMENT,CHANGE_PREVIOUS_EMPLOYMENT,NEW_CONCURRENT_EMPLOYMENT,CHANGE_EMPLOYER,AMENDED_PETITION,FULL_TIME_POSITION,H1B_DEPENDENT,WILLFUL_VIOLATOR,AGENT_REPRESENTING_EMPLOYER" -ForegroundColor Yellow
$small = $dataInCsv | select CASE_STATUS,TOTAL_WORKERS,NEW_EMPLOYMENT,CONTINUED_EMPLOYMENT,CHANGE_PREVIOUS_EMPLOYMENT,NEW_CONCURRENT_EMPLOYMENT,CHANGE_EMPLOYER,AMENDED_PETITION,FULL_TIME_POSITION,H1B_DEPENDENT,WILLFUL_VIOLATOR,AGENT_REPRESENTING_EMPLOYER
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
$small[0..$display] | ft

Write-Host "$(Get-Date): Selecting transforming case status to Outcome" -ForegroundColor Yellow
$small | %{if($_.CASE_STATUS -match "DENIED"){$_ | Add-Member NoteProperty "Outcome" 0 -Force}elseif($_.CASE_STATUS -eq "CERTIFIED"){$_ | Add-Member NoteProperty "Outcome" 1 -Force}else{$_ | Add-Member NoteProperty "Outcome" -1 -Force}}
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
$small[0..$display] | ft

Write-Host "$(Get-Date): Removing cases except denied and certified" -ForegroundColor Yellow
$filtered = $small |?{$_.Outcome -ne -1}
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
$filtered[0..$display] | ft

function getval($v){if($v -eq "Y" -or $v -eq "y"){return 1}elseif($v -eq "N" -or $v -eq "n"){return 0}else{return -1}}

Write-Host "$(Get-Date): Transforming Y and N to 1 and 0" -ForegroundColor Yellow
$filtered | %{$_ | Add-Member "FULL_TIME_POSITION_Num" $(getval $_.FULL_TIME_POSITION) -Force;$_ | Add-Member "H1B_DEPENDENT_Num" $(getval $_.H1B_DEPENDENT) -Force;$_ | Add-Member "WILLFUL_VIOLATOR_Num" $(getval $_.WILLFUL_VIOLATOR) -Force;$_ | Add-Member "AGENT_REPRESENTING_EMPLOYER_Num" $(getval $_.AGENT_REPRESENTING_EMPLOYER) -Force}
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
$filtered[0..$display] | ft

Write-Host "$(Get-Date): Selecting Coulmns with numeric values" -ForegroundColor Yellow
$lim = $filtered | select TOTAL_WORKERS,NEW_EMPLOYMENT,CONTINUED_EMPLOYMENT,CHANGE_PREVIOUS_EMPLOYMENT,NEW_CONCURRENT_EMPLOYMENT,CHANGE_EMPLOYER,AMENDED_PETITION,Outcome,FULL_TIME_POSITION_Num,H1B_DEPENDENT_Num,WILLFUL_VIOLATOR_Num,AGENT_REPRESENTING_EMPLOYER_Num
$lim | Export-Csv -Path "./transformed_all_cols.csv" -Encoding UTF8 -NoTypeInformation
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
$lim[0..$display] | ft

Write-Host "$(Get-Date): Checking if all numeric coulmns are valid i.e. data is numeric as well" -ForegroundColor Yellow
$props = $($lim[0] | Get-Member -Type NoteProperty).Name
$result = New-Object Collections.Generic.List[psobject]
$lim | %{$d = $_;$valid = $true;$props | %{$v=0;$r = [int]::TryParse($d.$_,[ref]$v); if($r -eq $true){$d.$_=$v}else{$d.$_=-1;$valid=$false} };if($valid){$result.Add($d)}}
$lim = $result.ToArray()
$lim | Export-Csv -Path "./transformed_all_valid.csv" -Encoding UTF8 -NoTypeInformation
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
$lim[0..$display] | ft

Write-Host "$(Get-Date): Separating training and validation sets and saving as csv" -ForegroundColor Yellow
$lim | Export-Csv -Path "./transformed_all.csv" -Encoding UTF8 -NoTypeInformation
$70 = [int]($lim.count*$trainingPercent)
$lim[0..$70] | Export-Csv -Path "./training_$trainingPercent.csv" -Encoding UTF8 -NoTypeInformation
$lim[$($70+1)..$($lim.count)] | Export-Csv -Path "./validation_$(1-$trainingPercent).csv" -Encoding UTF8 -NoTypeInformation
Write-Host "$(Get-Date): Done" -ForegroundColor Yellow
