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
		color = {r=255,g=25,b=25},
		name_color = {r=255,g=25,b=25}
	})
	
	
	CE_AddHook("OnEject", function(player)
		--AAAAAAA
	end)
	
	CE_AddStringSetting("vent_setting","Who Can Vent", 1, {"Impostors Only","Everybody","Nobody"})
	CE_AddToggleSetting("end_on_zero_only","Game Only ends on 0 Crew", false, {"True","False"})
	
	return {"Base","base"} --Display Name then Internal Name
end

function SelectRoles(players)
	return {{players[1],players[2],players[3],players[4]},{"impostor","crewmate","crewmate","crewmate"}}
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
	
	
	if (#impostors >= #crewmates) then
		CE_WinGame(CE_GetAllPlayersOnTeam(1,false),"default_impostor")
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
