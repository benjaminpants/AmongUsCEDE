function InitializeGamemode()
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
		layer = 1,
		team = 1,
		primary_valid_targets = VPT_Others,
		immune_to_light_affectors = true,
		color = {r=255,g=25,b=25},
		name_color = {r=255,g=25,b=25}
	})
	
	
	CE_AddRole({
		internal_name = "jester",
		name = "Jester",
		role_text = "",
		task_text = "Decieve the crew into thinking you are an impostor.",
		specials = {RS_Report},
		has_tasks = false,
		role_vis = RV_None,
		layer = 0,
		team = 2,
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
	
	CE_AddHook("OnEject", function(ejected)
		if (not CE_AmHost()) then return end
		if (ejected == nil) then return end
		print(ejected.role)
		if (ejected.role == "jester") then
			CE_WinGame({ejected},"default_disconnect")
		end
	end)
	
	CE_AddStringSetting("vent_setting","Who Can Vent", 1, {"Impostors Only","Everybody","Nobody"})
	CE_AddToggleSetting("end_on_zero_only","Game Only ends on 0 Crew", false, {"True","False"})
	CE_AddToggleSetting("vent_visibility","Visibility In Vents", true, {"Yes","No"})
	CE_AddIntSetting("jester_count","Jester Count","", 0, 1, 0, 1)
	
	return {"Roles","roles"} --Display Name then Internal Name
end

local function lerp(a, b, t)
	return a + (b - a) * t
end

function CalculateLightRadius(player,minradius,maxradius,lightsab) --lightsab is a float ranging from 0-1
	if (not CE_GetBoolSetting("vent_visibility")) then
		if (player.InVent) then
			return 0
		end
	end
	local mult = CE_GetInternalNumberSetting("crewmate_vision")
	if (player.Role == "impostor") then
		return maxradius * CE_GetInternalNumberSetting("impostor_vision")
	else
		mult = CE_GetInternalNumberSetting("crewmate_vision")
	end
	
	
	
	
	return lerp(minradius,maxradius,lightsab) * mult

end




function SelectRoles(players) --WHAT. THE FUCK. IS GOING ON.
	local RolesToGive = {}
	for i=1, CE_GetInternalNumberSetting("impostor_count") do
		table.insert(RolesToGive,"impostor")
	end
	
	local jest_count = CE_GetNumberSetting("jester_count")
	
	for i=1, jest_count do
		table.insert(RolesToGive,"jester")
	end
	
	
	for i=1, #players - #RolesToGive do
		table.insert(RolesToGive,"crewmate")
	end
	local Selected = {}
	local SelectedRoles = {}
	for i=1, #RolesToGive do
		local impid = math.random(1,#players) --randomly set the impostor id
		table.insert(Selected,players[impid]) --add it to the selected list
		table.insert(SelectedRoles,RolesToGive[i])
		table.remove(players,impid) --remove the chosen item from the playerinfo list
	end
	return {Selected,SelectedRoles} -- sets the sheriff's role
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
	local jesters = CE_GetAllPlayersOnTeam(2,true)
	local total_of_all = #jesters + #crewmates
	
	
	
	if (sab_loss) then
		CE_WinGame(CE_GetAllPlayersOnTeam(1,false),"default_impostor")
		return
	end
	
	if (#crewmates == 0 and #impostors == 0) then
		CE_WinGameAlt("stalemate")
		return
	end
	
	if (not CE_GetBoolSetting("end_on_zero_only")) then
		if (#impostors >= total_of_all) then
			CE_WinGame(CE_GetAllPlayersOnTeam(1,false),"default_impostor")
			return
		end
	else
		if (#crewmates == 0 and #impostors ~= 0) then
			CE_WinGame(CE_GetAllPlayersOnTeam(1,false),"default_impostor")
			return
		end
	end
	
	if (#impostors == 0) then
		CE_WinGame(CE_GetAllPlayersOnTeam(0,false),"default_crewmate")
		return
	end
	
	if (tasks_complete) then
		CE_WinGame(CE_GetAllPlayersOnTeam(0,false),"default_crewmate")
		return
	end
	
	

	
	
	

end


function OnHostRecieve(sender,id,params)
	

end


function OnUsePrimary(user,victim) --attention all gamers, feel free to call CanUsePrimary here, also this is ran on the host
	if (not CanUsePrimary(user,victim)) then
		return
	end
	CE_MurderPlayer(user,victim,true)
	
end
