function IsInRange(actor, target, near, far)		
	local distance = actor.depth + target.depth	
		
	if distance >= near and distance <= far then	
		return true
	end	
		
	return false	
end		



function AttackTarget(actor, target, attackType, amount)					
	
	if target.hp <= 0 then				
		return false			
	end				
					
	--bk test 케릭터의 정보를 걸러서 출력 
	local bPrint = false				
	--print("target.remoteId: "..target.remoteId)				
	if target.remoteId == 100 then --(콜로세움: remoteId 0=사람, 1=몹)						
		bPrint = true			
	end				
					
	if bPrint == true then
		print("============================================")
		print("Origin Damage: "..amount)			
	end				
					
					
	local damage = 0				
	local damageType = DamageType.None				
				
	if target.invincibleCount > 0 then -- bk, 무적				
		if bPrint == true then			
			print("target.invincibleCount: "..target.invincibleCount)		
		end			
		damageType = Enum.Include(damageType, DamageType.Invincible)    			
					
    elseif target.defensivePostureCount > 0 and Enum.Has(attackType, AttackType.Penetration) then --bk, 방어 + 관통 공격					
        
		if bPrint == true then					
			print("방어 + 관통 공격")		
		end			
		damageType = Enum.Include(damageType, DamageType.Penetration)
        damage = amount			
		target:ChangeState("knockback")
		if bPrint == true then					
			print("target:ChangeState(knockback)")		
		end			
					
		local damageReduce = GetDefenceRate(target.defence) - GetPenetrationRate(actor.penetration)--bk, GetRate은 Assets/StreamingAssets/LuaScripts/Formula.lua에서 참고함			
		if bPrint == true then			
			print("damageReduce: "..damageReduce.." / DefenceRate: "..GetDefenceRate(target.defence).." / PenetrationRate: "..GetPenetrationRate(actor.penetration))		
		end			
		
		--bk 추가
		--방막을 시도해서 회피를 포기(크리 발생 가능성 제거), 회피나 크리 계산을 하지 않는다.
		--bk

		if damageReduce >= 95 then -- 데미지감소율이 95%를 넘으면
            damage = damage * 0.05 -- 데미지 감소율 95% 로 적용
        elseif damageReduce >= 0 then -- 방어력 - 관통력 >=0 이면 계산
            damage = damage - damage * damageReduce/100
        else
            damage = damage -- 방어력 - 관통력 < 0 이면 0으로 적용	
        end
							
		if bPrint == true then			
			print("damageReduce 계산된 데미지 값: "..damage)		
		end			
					
					
		damage = damage * actor.attackFactor * target.damageFactor			
        if bPrint == true then					
			print("factor 계산 damage  "..damage.." / actor.attackFactor: "..actor.attackFactor.." / target.damageFactor: "..target.damageFactor)
		end			


	elseif target.defensivePostureCount == 2 and not Enum.Has(attackType, AttackType.Penetration) then --bk, 완전 방어, 방어시 회피, 크리티컬 배제				
		if bPrint == true then			
			print("완전 방어")		
		end			
        damageType = Enum.Include(damageType, DamageType.PerfectDefence)					
        damage = amount					
					
        local damageReduceRate = 0					
        damageReduceRate = (50 + GetDefenceRate(target.defence) - GetPenetrationRate(actor.penetration)) / 100					
        if bPrint == true then					
			print("damageReduceRate(50 보정): "..damageReduceRate.." / DefenceRate: "..GetDefenceRate(target.defence).." / PenetrationRate: "..GetPenetrationRate(actor.penetration))		
		end			
					
        if damageReduceRate > 1 then					
            damageReduceRate = 1					
        elseif damageReduceRate < 0.3 then --bk, 완전 방어시 데미지감소율 최소값					
            damageReduceRate = 0.3					
        end					
        					
        damage = damage * ( 1 - damageReduceRate )					
        if bPrint == true then					
			print("damageReduceRate 반영된 데미지 값: "..damage)		
		end			
					
		damage = damage * actor.attackFactor * target.damageFactor        			
        if bPrint == true then					
			print("factor 계산 damage  "..damage.." / actor.attackFactor: "..actor.attackFactor.." / target.damageFactor: "..target.damageFactor)	
		end			



    elseif target.defensivePostureCount == 1 and not Enum.Has(attackType, AttackType.Penetration) then--bk, 그냥 방어, 방어시 회피, 크리티컬 배제					
		if bPrint == true then			
			print("그냥 방어")		
		end			
        damageType = Enum.Include(damageType, DamageType.Defence)					
        damage = amount                					
    					
        local damageReduceRate = 0					
        damageReduceRate = (35 + GetDefenceRate(target.defence) - GetPenetrationRate(actor.penetration)) / 100					
		if bPrint == true then			
			print("damageReduceRate(35 보정): "..damageReduceRate.." / DefenceRate: "..GetDefenceRate(target.defence).." / PenetrationRate: "..GetPenetrationRate(actor.penetration))		
		end			
					
        if damageReduceRate > 1.65 then					
            damageReduceRate = 1					
        elseif damageReduceRate > 0.85 then					
        	damageReduceRate = 0.85 + 0.15*(damageReduceRate-0.85)/0.8				
        elseif damageReduceRate < 0.15 then -- 방어시 데미지감소율 최소값					
            damageReduceRate = 0.15					
        end					
        					
        damage = damage * ( 1 - damageReduceRate )					
		if bPrint == true then			
			print("damageReduceRate 반영된 데미지 값: "..damage)		
		end			
        damage = damage * actor.attackFactor * target.damageFactor					
        if bPrint == true then					
			print("factor 계산 damage  "..damage.." / actor.attackFactor: "..actor.attackFactor.." / target.damageFactor: "..target.damageFactor)
		end			
					


	else
		if bPrint == true then			
			print("특이사항 없이 맞았을 경우")		
		end			

		if Enum.Has(attackType, AttackType.Pure) then			
			if bPrint == true then		
				print("어택 타입 : Pure")	
			end		
			damageType = Enum.Include(damageType, DamageType.Pure)		
			damage = amount * actor.attackFactor * target.damageFactor		
		else
			if bPrint == true then		
				print("어택 타입 : Hit & Physics")	
			end		
			damageType = Enum.Include(damageType, DamageType.Hit)		
			damageType = Enum.Include(damageType, DamageType.Physics)		
		
			if Enum.Has(attackType, AttackType.Penetration) then		
				if bPrint == true then	
					print("어택 타입 : Penetration")
				end	
				damageType = Enum.Include(damageType, DamageType.Penetration)	
			end		
		
			local eva = math.random(1, 100)	
			local cri = math.random(1, 100)
			if eva > GetAccuracyRate(actor.accuracy) - GetEvasionRate(target.evasion) then
				damage = (amount - (amount / 100)) * 0.5
				damageType = Enum.Include(damageType, DamageType.Miss)
				if bPrint == true then
					print("회피 / AccuracyRate: "..GetAccuracyRate(actor.accuracy).." / EvasionRate: "..GetEvasionRate(target.evasion))
				end

		    elseif cri <= GetCriticalRate(actor.critical) - GetCriticalEvasionRate(target.criticalDefence) then			
				damage = (amount + (amount / 100)) * 1.5	
				damageType = Enum.Include(damageType, DamageType.Critical)	
				if bPrint == true then	
					print("크리티컬 / CriticalRate: "..GetCriticalRate(actor.critical).." / CriticalEvasionRate: "..GetCriticalEvasionRate(target.criticalDefence))
				end	
			else			
				damage = amount
				if bPrint == true then	
					print("보통 데미지(not 회피, not 크리티컬)")
				end	
			end			

            --bk, DamageReduce 를 고려하여 damage 량을 차감한다.					
			local damageReduce = GetDefenceRate(target.defence) - GetPenetrationRate(actor.penetration)		
			if bPrint == true then
				print("target.defence: "..target.defence)	
				print("damageReduce: "..damageReduce.." / DefenceRate: "..GetDefenceRate(target.defence).." / PenetrationRate: "..GetPenetrationRate(actor.penetration))	
			end		
            if damageReduce >= 95 then -- 데미지감소율이 100%를 넘으면					
            	damage = damage * 0.05 -- 데미지 감소율 95% 로 적용				
            	if bPrint == true then				
					print("damageReduce 100 넘음")
				end	
            elseif damageReduce >= 0 then -- 방어력 - 관통력 >=0 이면 계산					
                damage = damage - damage * damageReduce/100				
                if bPrint == true then					
					print("damageReduce 0 ~ 99.99.. 사이")
				end	
            else					
                damage = damage -- 방어력 - 관통력 < 0 이면 0으로 적용					
				if bPrint == true then	
					print("damageReduce 0 미만")
				end	
            end					

			if bPrint == true then	
				print("factor 계산 이전 damage  "..damage)
			end
			damage = damage * actor.attackFactor * target.damageFactor
			if bPrint == true then
				print("factor 계산 damage  "..damage.." / actor.attackFactor: "..actor.attackFactor.." / target.damageFactor: "..target.damageFactor)
			end	

		end
	end
	
	Damage(actor, target, damageType, damage)
	if bPrint == true then
		if damage > 0 then
			print("result true")
		else
			print("result false")
		end
	end
	return damage > 0
end


function AttackTargetRange(actor, target, attackType, amount, near, far)
	if IsInRange(actor, target, near, far) then
		return AttackTarget(actor, target, attackType, amount)
	end

	return false
end


function Damage(actor, target, damageType, amount)
	amount = math.floor(amount)
	if amount < 0 then
		amount = 0
	end
    

	--bk 스태프 저주 효과를 위한 코드
	local effect = actor:FindEffect("curse_4")
	if effect ~= nil then
		amount = amount/2
		if target.hp > 0 then
			actor:TakeDamage(actor, damageType, amount)	
		end
	end
	
	local effect = target:FindEffect("curse_3")
	if effect ~= nil then
		target:AttachEffect(actor, "curse_3", {1, amount})--폭발 데미지 증가
	end
	--bk end


	if target.hp > 0 then
		--print("Function.Lua/ Damage/ target:TakeDamage")
		target:TakeDamage(actor, damageType, amount)
	end
    
--    if actor.team == TeamType.Red then
--        print((math.floor(Game.battleTime*10)/10).."   "..math.floor((target.maxHp - target.hp)/Game.battleTime))
--    end
end


function Heal(actor, target, amount)
	amount = math.floor(amount)

	if target.hp > 0 and amount > 0 then
		target:TakeHeal(actor, amount)
	end
end

		  
function DifficultyDealy(actor)
	if actor.difficulty == 1 then
		return 3
	elseif actor.difficulty == 2 then
		return 2.5
	elseif actor.difficulty == 3 then
		return 2
	else
		return 2
	end
end


function DifficultyTimeout(actor)
	if actor.difficulty == 1 then
		return 120
	elseif actor.difficulty == 2 then
		return 90
	elseif actor.difficulty == 3 then
		return 60
	elseif actor.difficulty >= 4 then
		return 240
	else
		return 60
	end
end


function UsableSlot(actor, slot)
	if slot == 2 then
		if actor.target.weaponType == WeaponType.Axe then
			return	actor.target.skillController:GetRemainCooldown("axe_block")
		elseif actor.target.weaponType == WeaponType.Dagger then
			return	actor.target.skillController:GetRemainCooldown("dagger_counter_attack_2")
		elseif actor.target.weaponType == WeaponType.GreatSword then
			return	actor.target.skillController:GetRemainCooldown("greatsword_block")
		elseif actor.target.weaponType == WeaponType.Mace then
			return	actor.target.skillController:GetRemainCooldown("mace_block")
		elseif actor.target.weaponType == WeaponType.Staff then
			return	actor.target.skillController:GetRemainCooldown("staff_block")
		elseif actor.target.weaponType == WeaponType.Sword then
			return	actor.target.skillController:GetRemainCooldown("sword_block")
		elseif actor.target.weaponType == WeaponType.Sword then
			return	actor.target.skillController:GetRemainCooldown("spear_block")
		else
			return 0
		end

	elseif slot == 3 then
		if actor.target.weaponType == WeaponType.Axe then
			return	actor.target.skillController:GetRemainCooldown("axe_evade")
		elseif actor.target.weaponType == WeaponType.Dagger then
			return	actor.target.skillController:GetRemainCooldown("dagger_evade_attack")
		elseif actor.target.weaponType == WeaponType.GreatSword then
			return	actor.target.skillController:GetRemainCooldown("greatsword_evade_attack")
		elseif actor.target.weaponType == WeaponType.Mace then
			return	actor.target.skillController:GetRemainCooldown("mace_evade_attack")
		elseif actor.target.weaponType == WeaponType.Staff then
			return	actor.target.skillController:GetRemainCooldown("staff_evade")
		elseif actor.target.weaponType == WeaponType.Sword then
			return	actor.target.skillController:GetRemainCooldown("sword_evade_attack")
		elseif actor.target.weaponType == WeaponType.Spear then
			return	actor.target.skillController:GetRemainCooldown("spear_evade")
		else
			return 0
		end

	elseif slot == 4 then
		if actor.target.weaponType == WeaponType.Axe then
			return	actor.target.skillController:GetRemainCooldown("axe_interrupt_attack")
		elseif actor.target.weaponType == WeaponType.Dagger then
			return	actor.target.skillController:GetRemainCooldown("dagger_interrupt_attack")
		elseif actor.target.weaponType == WeaponType.GreatSword then
			return	actor.target.skillController:GetRemainCooldown("greatsword_interrupt_attack")
		elseif actor.target.weaponType == WeaponType.Mace then
			return	actor.target.skillController:GetRemainCooldown("mace_interrupt_attack")
		elseif actor.target.weaponType == WeaponType.Staff then
			return	actor.target.skillController:GetRemainCooldown("staff_interrupt_attack")
		elseif actor.target.weaponType == WeaponType.Sword then
			return	actor.target.skillController:GetRemainCooldown("sword_interrupt_attack")
		elseif actor.target.weaponType == WeaponType.Spear then
			return	actor.target.skillController:GetRemainCooldown("spear_interrupt_attack")
		else
			return 0
		end
	else
		return 0
	end
end

function GetRagePointOnHit(actor, amount)
	local ragePoint
	if actor.target.maxHp < actor.attackPower * 20 then
		ragePoint = amount / actor.target.maxHp * 35
	else
		ragePoint = (amount / (actor.attackPower * 20)) * 35
	end
	
	if ragePoint > 0 then
		return actor:AddRagePoint(ragePoint)
	else
		return actor:AddRagePoint(0)
	end
end


function GetRagePointOnTakeDamage(actor, amount)
	return actor:AddRagePoint(amount/actor.maxHp*60)
end