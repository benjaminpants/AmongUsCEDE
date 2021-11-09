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
		internal_name = "hawk",
		name = "Hawk-Eyed",
		role_text = "Use your increased vision to find the <color=#FF0000FF>Impostor.</color>",
		specials = {RS_Report},
		has_tasks = true,
		layer = 255, --special layer that shows everyone regardless of layer
		team = 0,
		role_vis = RV_None,
		color = {r=120, g=86, b=60},
		immune_to_light_affectors = true
	})
	
	CE_AddRole({
		internal_name = "sheriff",
		name = "Sheriff",
		role_text = "Track and Kill the Impostor.",
		specials = {RS_Primary,RS_Report},
		has_tasks = true,
		layer = 255, --special layer that shows everyone regardless of layer
		team = 0,
		role_vis = RV_None,
		color = {r=255,g=216,b=0}
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
		internal_name = "griefer",
		name = "Griefer",
		role_text = "Make someone look like they killed you.",
		task_text = "Make someone look guilty.",
		specials = {RS_Primary,RS_Vent,RS_Report},
		has_tasks = false,
		role_vis = RV_SameLayer,
		layer = 1,
		team = 1,
		primary_valid_targets = VPT_Others,
		immune_to_light_affectors = true,
		color = {r=87, g=85, b=42}
	})
	
	
	CE_AddRole({
		internal_name = "jester",
		name = "Jester",
		role_text = "",
		task_text = "Decieve the crew into thinking you are an impostor.",
		specials = {RS_Report},
		has_tasks = false,
		role_vis = RV_Script,
		layer = 0, --special layer that shows no one despite the layer they are on
		team = 2,
		primary_valid_targets = VPT_Others,
		immune_to_light_affectors = false,
		color = {r=129,g=41,b=139}
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
		if (ejected.Role == "jester") then
			CE_WinGame({ejected},"default_disconnect")
		end
	end)
	
	CE_AddHook("GetRemainText", function(ejected)
		local imp_sub = 0
		if (ejected.Role == "impostor") then
			imp_sub = 1
		end
		return (#CE_GetAllPlayersOnTeam(1,true) - imp_sub) .. " impostors remain."
	end)
	
	CE_AddHook("OnPlayerDeath", function(victim,reason)
		if (not CE_AmHost()) then return end
		if (victim.role == "sheriff" and CE_GetBoolSetting("ce_sheriff_behavior")) then
			ReAssignSheriff(victim)
		end
	end)
	
	CE_AddStringSetting("vent_setting","Who Can Vent", 1, {"Impostors Only","Everybody","Nobody"})
	CE_AddToggleSetting("end_on_zero_only","Game Only ends on 0 Crew", false, {"True","False"})
	CE_AddToggleSetting("vent_visibility","Visibility In Vents", true, {"Yes","No"})
	CE_AddIntSetting("sheriff_count","Sheriff Count","", 0, 1, 0, 2)
	CE_AddToggleSetting("ce_sheriff_behavior","CE Sheriff Behavior", true, {"Enabled","Disabled"})
	CE_AddIntSetting("jester_count","Jester Count","", 0, 1, 0, 1)
	CE_AddToggleSetting("imps_see_jester","Impostors See Jester", true, {"Yes","No"})
	CE_AddIntSetting("griefer_count","Griefer Count","", 0, 1, 0, 2)
	CE_AddIntSetting("hawk_count","Hawk-Eyed Count","", 0, 1, 0, 4)
	CE_AddFloatSetting("hawk_vision","Hawk-Eyed Vision","", 2, 0.25, 0.25, 5)
	
	return {"Roles","roles"} --Display Name then Internal Name
end

function GetValidReplacableRole(dodge)
	local players = CE_GetAllPlayers(true)
	local valid_players = {}
	for	i=1, #players do
		if (players[i].Role == "crewmate") then
			if (dodge == nil) then
				table.insert(valid_players,players[i])
			else
				if (players[i].ID ~= dodge.ID) then
					table.insert(valid_players,players[i])
				end
			end
		end
	end
	if (#valid_players == 0) then return nil end
	return valid_players[math.random(1,#valid_players)]
end

function ReAssignSheriff(victim,dodge)
	if (not CE_AmHost()) then return end
	local valid_replacable = GetValidReplacableRole(dodge)
	if (valid_replacable ~= nil) then
		CE_SetRoles({victim,valid_replacable},{"crewmate","sheriff"})
	end
end


function IsRoleConsideredBad(role)
	return (role == "impostor" or role == "witch" or role == "jester")
end


function CanSeeRole(name,owner,viewer)
	if (name == "jester" and CE_GetBoolSetting("imps_see_jester")) then
		if (viewer.Role == "impostor" or viewer.Role == "griefer") then
			return true
		end
	end
	return false
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
	if (player.Role == "hawk") then
		return maxradius * CE_GetNumberSetting("hawk_vision")
	end
	if (player.Role == "impostor" or player.Role == "griefer") then
		return maxradius * CE_GetInternalNumberSetting("impostor_vision")
	else
		mult = CE_GetInternalNumberSetting("crewmate_vision")
	end
	
	
	
	
	return lerp(minradius,maxradius,lightsab) * mult

end




function SelectRoles(players)
	local RolesToGive = {}
	for i=1, CE_GetInternalNumberSetting("impostor_count") do
		table.insert(RolesToGive,"impostor")
	end
	
	local jest_count = CE_GetNumberSetting("jester_count")
	local sheriff_count = CE_GetNumberSetting("sheriff_count")
	local griefer_count = CE_GetNumberSetting("griefer_count")
	local hawk_count = CE_GetNumberSetting("hawk_count")
	
	for i=1, jest_count do
		table.insert(RolesToGive,"jester")
	end
	
	for i=1, sheriff_count do
		table.insert(RolesToGive,"sheriff")
	end
	
	for i=1, griefer_count do
		table.insert(RolesToGive,"griefer")
	end
	
	for i=1, hawk_count do
		table.insert(RolesToGive,"hawk")
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
	if (user.Role == "sheriff") then
		return true
	end
	if (user.ID == victim.ID) then --let them commit death on themselves lol, should probs be removed for other roles though lol
		return true
	end
	if (user.Role == "impostor" or user.Role == "griefer") then
		if (victim.Layer ~= 1 and (victim.Role ~= "jester" or not CE_GetBoolSetting("imps_see_jester"))) then
			return true
		end
	end
	
	return false
end

function CheckEndCriteria(tasks_complete, sab_loss)
	local impostors = CE_GetAllPlayersOnTeam(1,true)
	local crewmates = CE_GetAllPlayersOnTeam(0,true)
	local jesters = CE_GetAllPlayersOnTeam(2,true)
	local total_of_all = #jesters + #crewmates
	
	--current goal conditions:
	--sab loss
	--nobody on any team
	--imp win due to kills
	--no impostors win
	--tasks complete
	--jester stalemate
	
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
		if (total_of_all == 0 and #impostors ~= 0) then
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
	
	if (#impostors ~= 0) then
		if ((#jesters > #crewmates) and #crewmates <= #impostors) then
			CE_WinGameAlt("stalemate")
		end
	end
	
	

	
	
	

end


function OnHostRecieve(sender,id,params)
	

end


function OnUsePrimary(user,victim) --attention all gamers, feel free to call CanUsePrimary here, also this is ran on the host
	if (not CanUsePrimary(user,victim)) then
		return
	end
	if (user.Role == "sheriff") then
		if (CE_GetBoolSetting("ce_sheriff_behavior")) then
			ReAssignSheriff(user,victim)
		else
			if (not IsRoleConsideredBad(victim.Role)) then
				CE_MurderPlayer(user,user,false)
				return
			end
		end
	end
	
	if (user.role ~= "griefer") then
		CE_MurderPlayer(user,victim,true)
	else
		CE_MurderPlayer(victim,user,true)
	end
	
end
