$rooms = @(
    @{ RoomName = "Grupprum 1"; Capacity = 4 },
    @{ RoomName = "Grupprum 2"; Capacity = 6 },
    @{ RoomName = "Sal A"; Capacity = 30 },
    @{ RoomName = "Sal B"; Capacity = 45 },
    @{ RoomName = "Datorsal 1"; Capacity = 20 }
)

foreach ($room in $rooms) {
    $json = $room | ConvertTo-Json
    Invoke-RestMethod -Uri "https://app-bookingapi.azurewebsites.net/api/rooms" -Method Post -Body $json -ContentType "application/json"
}
