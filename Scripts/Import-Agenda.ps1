[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$response = Invoke-WebRequest 'https://symposium.sitecore.com/data/routes/agenda/en.json' -UseBasicParsing
$response = $response | select -expand Content

$defaultEncoding = [System.Text.Encoding]::GetEncoding('ISO-8859-1')
$utf8Bytes = $defaultEncoding.GetBytes($response)
$decoded = [System.Text.Encoding]::UTf8.GetString($utf8bytes)

$source = $decoded | ConvertFrom-Json

$days = ($source.placeholders.main | ?{ $_.componentName -eq 'Agenda' })[0].fields.days

$sessions = $days | %{
    $_.fields.date.value -match '^(\d+)'
    
    $date = [DateTime]::Parse("2018-09-30").AddDays([int]::Parse($matches[0]))
    
    $_.fields.sessions | %{
        $session = $_.fields
        $session.PSObject.Properties | %{
            if ($_.Value -and ($_.Value.GetType().FullName -eq "System.Management.Automation.PSCustomObject")) {
                $session.($_.Name) = $_.Value.value
            }
        }
        
        if($session.speakers) {
            $speakers = $session.speakers | %{
                $_.fields.name.value + ", " + $_.fields.jobTitle.value + ", " + $_.fields.company.value
            }
            $session.speakers = $speakers -join "<br>" | Out-String
        }
        
        $session.startTime = $date.Add([DateTime]::Parse($session.startTime).TimeOfDay)
        $session.endTime = $date.Add([DateTime]::Parse($session.endTime).TimeOfDay)
        $session | Add-Member date $date
        
        $session
    }
} | ?{ $_.GetType().FullName -eq "System.Management.Automation.PSCustomObject" }

$sessions | %{
    $itemName = ((Get-Culture).TextInfo.ToTitleCase($_.title) -replace '[^A-Za-z0-9]+','')[0..99] -join ''
    $itemPath = "master:\content\Sessions\$itemName"
    
    "Importing: $itemName"
    
    if(Test-Path $itemPath) {
        $item = Get-Item -Path $itemPath
    } else {
        $item = New-Item -Path "master:\content\Sessions" -Name $itemName -ItemType "Sym2018/Feature/Agenda/Symposium Session"
    }
    
    $item.Title = $_.title
    if($_.description) { $item.Description = $_.description }
    if($_.date) { $item.'Session Date' = $_.date.ToString('D', [CultureInfo]::CreateSpecificCulture("en-us")) }
    if($_.startTime) { $item.'Start Time' = $_.startTime }
    if($_.endTime) { $item.'End Time' = $_.endTime }
    if($_.room) { $item.Room = $_.room }
    if($_.track) { $item.Track = $_.track }
    if($_.type) { $item.Type = $_.type }
    if($_.speakers) { $item.Speakers = $_.speakers }
}

""
"DONE!"
