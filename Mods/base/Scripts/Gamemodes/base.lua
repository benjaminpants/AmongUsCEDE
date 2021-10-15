function InitializeGamemode()
	print("ayo is the custom print function working")
	CE_AddRole({
		internal_name = "crewmate",
		name = "Crewmate",
		role_text = "There are impostors Among Us.",
		specials = {RS_Report},
		has_tasks = true,
		layer = 255, --special layer that shows everyone regardless of layer
		team = 0,
		role_vis = RV_None,
		color = {r=140,g=255,b=255},
		name_color = {r=255,g=255,b=255}
	})
	
	CE_AddRole({
		internal_name = "impostor",
		name = "Impostor",
		role_text = "",
		task_text = "Sabotage and kill everyone.",
		specials = {RS_Primary,RS_Sabotage,RS_Vent,RS_Report},
		has_tasks = false,
		role_vis = RV_SameLayer,
		layer = 0,
		team = 1,
		primary_valid_targets = VPT_Others,
		immune_to_light_affectors = true,
		color = {r=255,g=25,b=25},
		name_color = {r=255,g=25,b=25}
	})
	
	
	CE_AddHook("CanVent", function(player, usual)
		local vent_set = CE_GetNumberSetting("vent_setting")
		if (vent_set == 1) then
			return usual
		elseif (vent_set == 2) then
			return true
		else
			return false
		end
		return usual
	end)
	
	CE_AddStringSetting("vent_setting","Who Can Vent", 1, {"Impostors Only","Everybody","Nobody"})
	CE_AddToggleSetting("end_on_zero_only","Game Only ends on 0 Crew", false, {"True","False"})

	print(CE_Random.GetRandom())
	
	return {"Base","base"} --Display Name then Internal Name
end

local function lerp(a, b, t)
	return a + (b - a) * t
end

function CalculateLightRadius(player,minradius,maxradius,lightsab) --lightsab is a float ranging from 0-1
	local mult = CE_GetInternalNumberSetting("crewmate_vision")
	if (player.Role == "impostor") then
		return maxradius * CE_GetInternalNumberSetting("impostor_vision")
	else
		mult = CE_GetInternalNumberSetting("crewmate_vision")
	end
	
	
	
	
	return lerp(minradius,maxradius,lightsab) * mult

end

local function insert_table_fixed(tab, addend) --work around for broken moonsharp stuff, TODO: fix moonsharp lol
	tab[#tab + 1] = addend
end





function SelectRoles(players) --WHAT. THE FUCK. IS GOING ON.
	
	local roles_to_select = {}
	local players_finished = {}
	for i=1, CE_GetInternalNumberSetting("impostor_count") do
		insert_table_fixed(roles_to_select,"impostor")
	end
	print("got past imp")
	for i=1, #players - #roles_to_select do
		insert_table_fixed(roles_to_select,"crewmate")
	end
	print("got past crew")
	for i=1, #roles_to_select do
		local id = math.random(1,#players)
		print("selected id")
		while players[id] == nil do
			print("reselecting id")
			local id = math.random(1,#players)
		end
		print("adding to table")
		insert_table_fixed(players_finished,players[id])
		players[id] = nil
	end
	
	
	return {players_selected,roles_to_select}
end

function CanUsePrimary(user,victim)
	if (user.PlayerId == victim.PlayerId) then --let them commit death on themselves lol, should probs be removed for other roles though lol
		return true
	end
	if (user.Role ~= "impostor") then
		return false
	end
	if (user.Role == "impostor") then
		if (victim.Role == "impostor") then
			return false
		end
	end
	return true
end

function CheckEndCriteria(tasks_complete, sab_loss)
	local impostors = CE_GetAllPlayersOnTeam(1,true)
	local crewmates = CE_GetAllPlayersOnTeam(0,true)
	
	if (#crewmates == 0 and #impostors == 0) then
		CE_WinGameAlt("stalemate")
	end
	
	if (not CE_GetToggleSetting("end_on_zero_only")) then
		if (#impostors >= #crewmates) then
			CE_WinGame(CE_GetAllPlayersOnTeam(1,false),"default_impostor")
		end
	else
		if (#crewmates == 0 and #impostors ~= 0) then
			CE_WinGame(CE_GetAllPlayersOnTeam(1,false),"default_impostor")
		end
	end
	
	if (#impostors == 0) then
		CE_WinGame(CE_GetAllPlayersOnTeam(0,false),"default_crewmate")
	end
	
	if (tasks_complete) then
		CE_WinGame(CE_GetAllPlayersOnTeam(0,false),"default_crewmate")
	end
	
	
	
	

end


function OnUsePrimary(user,victim) --attention all gamers, feel free to call CanUsePrimary here, also this is ran on the host
	if (not CanUsePrimary(user,victim)) then
		return
	end
	CE_MurderPlayer(user,victim,true)
	
end
