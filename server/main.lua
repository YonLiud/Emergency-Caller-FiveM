RegisterNetEvent('ec:new')
AddEventHandler('ec:new', function(name, description, cords)
    TriggerClientEvent('ec:new', -1, name, description, cords)
end)

-- this is literally all of the script istg