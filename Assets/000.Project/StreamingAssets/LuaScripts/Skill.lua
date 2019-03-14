local attackCooltime = 0.2
local blockCooltime = 3
local strongAttackCooltime = 5
local evadeAttackCooltime = 10
local interruptAttackCooltime = 15
local penetrateAttackCooltime = 5
local inevitableAttackCooltime= 15


local ragePoint01 = 40 -- 관통 스킬 레이지 포인트 소모량
local ragePoint02 = 100-- 확정 스킬 레이지 포인트 소모량
local ragePoint03 = 30 -- 특수 스킬 레이지 포인트 소모량
local ragePoint04 = 10 -- 망치 회전 스킬 레이지 포인트 소모량

function testCooltime()
    attackCooltime = 1
    blockCooltime = 1
    strongAttackCooltime = 1
    penetrateAttackCooltime = 1
    inevitableAttackCooltime = 1
end


function takeMonsterValue(endFrame, Hit)
	if not Hit then
		return 1
	else
    	if endFrame <= 120 then
	    	return (endFrame/60 * (0.9 + Hit/10))/Hit -- 120프레임 이하에서는 프레임길이에 비례해서 데미지 / 1타당 데미지 10% 추가
	    	--return 1
   		elseif endFrame <= 240 then
			return ((2 + (endFrame-120)/2/60) * (0.9 + Hit/10))/Hit -- 120~240프레임에서는 프레임길이/2 비례해서 데미지 / 1타가 넘어갈 때마다 10% 가산
			--return 1
   		elseif endFrame > 240 then
			return ((3 + (endFrame-240)/3/60) * (0.9 + Hit/10))/Hit -- 240프레임이 넘어간 경우 프레임길이/3 비례해서 데미지 / 1타가 넘어갈 때마다 10% 가산
			--return 1
	   	end
	end
end


function takeCooltime(actor, cooltime, slot)
    --testCooltime()
    
    local slotType = slot
    slot = slot - 1
    if slotType == 4 then
        --return cooltime - math.ceil(actor.skillController:GetSkillSlotLevel(slot)/2) * 0.4 -- 1 3 5.. 레벨마다(2렙간격) 쿨 0.4초 감소
        return cooltime
    elseif slotType == 5 then
        --return cooltime - math.ceil(actor.skillController:GetSkillSlotLevel(slot)/2) * 0.6 -- 1 3 5.. 레벨마다(2렙간격) 쿨 0.6초 감소
        return cooltime
    else
        return cooltime
    end
end


function takeValue(actor, level, slotType, slot, revision)
    slot = slot - 1
    
    if Game then
		if Game.gameMode == GameMode.PVP then
			level = 0.8 + level * 0.2
		end
	end
    
    if slotType == 1 then
        --return math.floor( (actor.attackPower + level * 10 ) * (1 + actor.skillController:GetSkillSlotLevel(slot) * 0.05) * revision ) -- 평타
        return math.floor( (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision ) -- 평타
    elseif slotType == 2 then
        --return math.floor( (actor.attackPower + level * 10 ) * (1 + actor.skillController:GetSkillSlotLevel(slot) * 0.05) * revision ) -- 강공격
        return math.floor( (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision ) -- 강공격
    elseif slotType == 3 then
        --return math.floor( (level * 70 ) * (1 + actor.skillController:GetSkillSlotLevel(slot) * 0.05) * revision ) -- 방어
		--print("actor.remoteId: ".. actor.remoteId.."  ,skill level: "..level.."  ,slotType: "..slotType.."  ,slot: "..slot.."  ,attackPower: "..actor.attackPower.."  ,revision: "..revision)
        --print("defence: ".. (level * 70 ) * (1 + 0 * 0.05) * revision)
        return math.floor( (level * 70 ) * (1 + 0 * 0.05) * revision ) -- 방어
    elseif slotType == 4 then
        --return math.floor( (actor.attackPower + level * 10 ) * (1 + actor.skillController:GetSkillSlotLevel(slot) * 0.05) * revision ) -- 회피
        return math.floor( (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision ) -- 회피
    elseif slotType == 5 then
        --return math.floor( (actor.attackPower + level * 10 ) * (1 + actor.skillController:GetSkillSlotLevel(slot) * 0.05) * revision ) -- 차단
        return math.floor( (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision ) -- 차단
    elseif slotType == 6 then
        --return math.floor( (actor.attackPower + level * 10 ) * (1 + actor.skillController:GetSkillSlotLevel(3) * 0.05) * revision ) -- 관통
        return math.floor( (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision ) -- 관통
    elseif slotType == 7 then
        --return math.floor( (actor.attackPower + level * 10 ) * (1 + actor.skillController:GetSkillSlotLevel(4) * 0.05) * revision ) -- 확정
		--print("actor.remoteId: ".. actor.remoteId.."  ,skill level: "..level.."  ,slotType: "..slotType.."  ,slot: "..slot.."  ,attackPower: "..actor.attackPower.."  ,revision: "..revision)
        --print("damage: ".. (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision)
		return math.floor( (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision ) -- 확정
	elseif slotType == 8 then
		return math.floor( (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision ) -- 확정
	elseif slotType == 9 then
		return math.floor( (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision )
	elseif slotType == 10 then
		return math.floor( (actor.attackPower + level * 20 ) * (1 + 0 * 0.05) * revision )
    else
	    return 0
    end
end

------------------------------------- greatsword ------------------------------------------------------------

function greatsword_rl_attack(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.1 * 0.9							--local revision = 1.08 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function greatsword_lr_attack(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.1							--local revision = 1.08
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function greatsword_counter_attack(actor, level)
	local cooltime = attackCooltime
    local slot = 7
    local revision = 3.1							--local revision = 2.93
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function greatsword_lr_spin_attack(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.9							--local revision = 1.73
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function greatsword_cut_down(actor, level)
	local cooltime = strongAttackCooltime
    local slot = 2
    local revision = 6.3							--local revision = 6.23
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function greatsword_sting(actor, level)
	local cooltime = attackCooltime
    local slot = 2
    local revision = 3.4							--local revision = 3.33
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function greatsword_block(actor, level)
	local cooltime = blockCooltime
    local slot = 3
    local revision = 1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
            takeValue(actor, level, slot, slot, revision),
            takeValue(actor, level+1, slot, slot, revision),
		}
end


function greatsword_evade_attack(actor, level)
	local cooltime = evadeAttackCooltime
    local slot = 4
    local revision = 3.4							--local revision = 3.33
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function greatsword_interrupt_attack(actor, level)
	local cooltime = interruptAttackCooltime 
    local slot = 5
    local revision = 2.5							--local revision = 2.43
    
    return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function greatsword_penetrate_attack(actor, level)
	local cooltime = penetrateAttackCooltime
    local slot = 6
    local revision = 4.8							--local revision = 4.75
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint01,
		}
end


function greatsword_inevitable_attack(actor, level)
	local cooltime = inevitableAttackCooltime
    local slot = 7
    local revision = 7.1							--local revision = 7.08
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint02,
		}
end 

function greatsword_wildly(actor, level)
	local addwildly = 15
	local slot = 8
	local revision = 0.06
	return
		{
			[0] = addwildly,            
			takeValue(actor, level, slot, slot, revision) + level * 8,
			takeValue(actor, level+1, slot, slot, revision) + (level+1) * 8,
		}
end

------------------------------------- hammer ----------------------------------------------------------

function hammer_attack_1(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.05 * 1.1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function hammer_attack_2(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.05 * 1.15
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function hammer_strong_attack_1(actor, level)
	local cooltime = 5 --strongAttackCooltime
    local slot = 2
    local revision = 2.13
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function hammer_strong_attack_2(actor, level)
	local cooltime = 5 --strongAttackCooltime
    local slot = 2
    local revision = 2.93
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

function hammer_spin_attack(actor, level)
	local cooltime = 15
    local slot = 6
    local revision = 1.93
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint04,
		}
end


function hammer_block(actor, level)
	local cooltime = blockCooltime
    local slot = 3
    local revision = 1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function hammer_counter_attack(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 2.08
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function hammer_evade_attack(actor, level)
	local cooltime = evadeAttackCooltime
    local slot = 4
    local revision = 2
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function hammer_interrupt_attack(actor, level)
	local cooltime = interruptAttackCooltime
    local slot = 5
    local revision = 1.2
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function hammer_penetrate_attack(actor, level)
	local cooltime = penetrateAttackCooltime
    local slot = 6
    local revision = 3.5
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint01,
		}
end


function hammer_inevitable_attack(actor, level)
	local cooltime = inevitableAttackCooltime
    local slot = 7
    local revision = 6.55
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint02,
		}
end

------------------------------------- mace ------------------------------------------------------------

function mace_attack_1(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.94 * 0.9 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function mace_attack_2(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.94 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function mace_attack_3(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.94 * 1.1 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function mace_counter_attack(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 2 * 1.0
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function mace_strong_attack(actor, level)
	local cooltime = 5 --strongAttackCooltime
    local slot = 2
    local revision = 2.12 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function mace_block(actor, level)
	local cooltime = blockCooltime
    local slot = 3
    local revision = 1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function mace_evade_attack(actor, level)
	local cooltime = evadeAttackCooltime
    local slot = 4
    local revision = 1.93 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function mace_interrupt_attack(actor, level)
	local cooltime = interruptAttackCooltime
    local slot = 5
    local revision = 1 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end



function mace_penetrate_attack(actor, level)
	local cooltime = penetrateAttackCooltime
    local slot = 6
    local revision = 3.03 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint01,
		}
end


function mace_inevitable_attack(actor, level)
	local cooltime = inevitableAttackCooltime
    local slot = 7
    local revision = 15.5 / 30 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint02,
		}
end

------------------------------------- dagger ------------------------------------------------------------

function dagger_attack_1(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.94 * 0.8 / 2
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function dagger_attack_2(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.94 * 0.85 / 2
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function dagger_attack_3(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.94 * 1.35 / 3
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function dagger_counter_attack_1(actor, level)
	local cooltime = blockCooltime * 2
    local slot = 2
    local revision = 1.37
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, 1, slot, revision),
			takeValue(actor, level+1, 1, slot, revision),
			takeValue(actor, level, 3, slot, 1),
			takeValue(actor, level+1, 3, slot, 1),
		}
end


function dagger_counter_attack_1_counter(actor, level)
	local cooltime = attackCooltime
    local slot = 2
    local revision = 1.8 / 3
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, 1, slot, revision),
			takeValue(actor, level+1, 1, slot, revision),
		}
end


function dagger_counter_attack_2(actor, level)
	local cooltime = blockCooltime * 2 
    local slot = 3
    local revision = 1.38
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, 1, slot, revision),
			takeValue(actor, level+1, 1, slot, revision),
			takeValue(actor, level, 3, slot, 1),
			takeValue(actor, level+1, 3, slot, 1),
		}
end


function dagger_counter_attack_2_counter(actor, level)
	local cooltime = attackCooltime
    local slot = 3
    local revision = 1.03
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, 1, slot, revision),
			takeValue(actor, level+1, 1, slot, revision),
		}
end


function dagger_evade_attack(actor, level)
	local cooltime = evadeAttackCooltime
    local slot = 4
    local revision = 1.7
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function dagger_interrupt_attack(actor, level)
	local cooltime = interruptAttackCooltime
    local slot = 5
    local revision = 1.38
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function dagger_penetrate_attack(actor, level)
	local cooltime = penetrateAttackCooltime
    local slot = 6
    local revision = 2.18
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint01,
		}
end


function dagger_inevitable_attack(actor, level)
	local cooltime = inevitableAttackCooltime
    local slot = 7
    local revision = 4.47 / 4
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint02,
		}
end


-------------------------------------  sword  ------------------------------------------------------------

function sword_attack_1(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.88 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function sword_attack_2(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.88
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function sword_attack_3(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.88 * 1.1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function sword_block_attack(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.23
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function sword_block_attack2(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.43
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function sword_strong_attack(actor, level)
	local cooltime = 8 --strongAttackCooltime
    local slot = 2
    local revision = 2.98
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function sword_block_strong_attack(actor, level)
	local cooltime = 6
    local slot = 2
    local revision = 2.35
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function sword_block(actor, level)
	local cooltime = blockCooltime
    local slot = 3
    local revision = 1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function sword_evade_attack(actor, level)
	local cooltime = evadeAttackCooltime
    local slot = 4
    local revision = 2.3
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function sword_interrupt_attack(actor, level)
	local cooltime = interruptAttackCooltime
    local slot = 5
    local revision = 1.02
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

function sword_penetrate_attack(actor, level)
	local cooltime = penetrateAttackCooltime
    local slot = 6
    local revision = 4.90
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint01,
		}
end


function sword_inevitable_attack(actor, level)
	local cooltime = inevitableAttackCooltime
    local slot = 7
    local revision = 3.54
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint02,
		}
end


-------------------------------------  axe  ------------------------------------------------------------

function axe_attack_1(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.04 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function axe_attack_2(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.04
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function axe_attack_3(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.04 * 1.1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function axe_strong_attack(actor, level)
	local cooltime = strongAttackCooltime
    local slot = 2
    local revision = 0.9
	return
		{
			[0] = takeCooltime(actor, 15 + (level-1) * 0.04 , slot),
            takeCooltime(actor, 15 + level * 0.04, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function axe_block(actor, level)
	local cooltime = blockCooltime
    local slot = 3
    local revision = 1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
        }
end


function axe_evade(actor, level)
	local cooltime = evadeAttackCooltime
    local slot = 4
    local revision = 0.2

	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision) + level * 8,
			takeValue(actor, level+1, slot, slot, revision) + (level+1) * 8,
		}
end

function axe_interrupt_attack(actor, level)
	local cooltime = interruptAttackCooltime
    local slot = 5
    local revision = 1.33
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function axe_penetrate_attack(actor, level)
	local cooltime = penetrateAttackCooltime
    local slot = 6
    local revision = 4.67
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint01,
		}
end


function axe_inevitable_attack(actor, level)
	local cooltime = inevitableAttackCooltime
    local slot = 7
    local revision = 0.62
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint02,
		}
end




function axe_fury(actor, level)
	local cooltime = 1
    local slot = 7
    local revision = 1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			2 + (level-1) * 0.04,
			2 + level * 0.04,
		}
end


------------------------------------- staff ------------------------------------------------------------

function staff_attack_1(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.05 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function staff_attack_2(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 1.05
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

function staff_strong_attack(actor, level)
	local cooltime = strongAttackCooltime
	local slot = 2
	local revision = 3.1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

-- function staff_meditation(actor, level)
	-- local cooltime = attackCooltime
    -- local slot = 6
    -- local revision01 = 3.5
    -- local revision02 = 9.3
	-- return
		-- {
			-- [0] = takeCooltime(actor, cooltime, slot),
            -- takeCooltime(actor, cooltime, slot),
			-- takeValue(actor, level, slot, slot, revision01),
			-- takeValue(actor, level+1, slot, slot, revision01),
            -- takeValue(actor, level, slot, slot, revision02),
            -- takeValue(actor, level+1, slot, slot, revision02),
            -- 2,
            -- 2,
		-- }
-- end

function staff_brave(actor, level)
	local cooltime = attackCooltime
	local slot = 7
	local revision = 1.2
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

function staff_curse_1(actor, level)
	local cooltime = 2
	local slot = 6
	local revision = 0.95
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

function staff_curse_2(actor, level)
	local cooltime = 2
	local slot = 6
	local revision = 1.25
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

function staff_curse_3(actor, level)
	local cooltime = 2
	local slot = 6
	local revision = 1.45
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

function staff_curse_4(actor, level)
	local cooltime = 2
	local slot = 6
	local revision = 2.0
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

-- function staff_meditation_1_end(actor, level)
	-- local cooltime = penetrateAttackCooltime
    -- local slot = 2
    -- local revision = 3.43
	-- return
		--{
			-- [0] = takeCooltime(actor, cooltime, slot),
            -- takeCooltime(actor, cooltime, slot),
			-- takeValue(actor, level, slot, slot, revision),
			-- takeValue(actor, level+1, slot, slot, revision),
		-- }
-- end


-- function staff_meditation_2_end(actor, level)
	-- local cooltime = penetrateAttackCooltime
    -- local slot = 2
    -- local revision = 9.32
	-- return
		-- {
			-- [0] = takeCooltime(actor, cooltime, slot),
            -- takeCooltime(actor, cooltime, slot),
			-- takeValue(actor, level, slot, slot, revision),
			-- takeValue(actor, level+1, slot, slot, revision),
		-- }
-- end


-- function staff_meditation_3_end(actor, level)
	-- local cooltime = penetrateAttackCooltime
    -- local slot = 2
    -- local revision = 18.37
	-- return
		-- {
			-- [0] = takeCooltime(actor, cooltime, slot),
            -- takeCooltime(actor, cooltime, slot),
			-- takeValue(actor, level, slot, slot, revision),
			-- takeValue(actor, level+1, slot, slot, revision),
		-- }
-- end


-- function staff_meditation_4_end(actor, level)
	-- local cooltime = penetrateAttackCooltime
    -- local slot = 2
    -- local revision = 24.52
	-- return
		-- {
			-- [0] = takeCooltime(actor, cooltime, slot),
            -- takeCooltime(actor, cooltime, slot),
			-- takeValue(actor, level, slot, slot, revision),
			-- takeValue(actor, level+1, slot, slot, revision),
		-- }
-- end


-- function staff_meditation_5_end(actor, level)
	-- local cooltime = penetrateAttackCooltime
    -- local slot = 2
    -- local revision = 26.52 / 10
	-- return
		-- {
			-- [0] = takeCooltime(actor, cooltime, slot),
            -- takeCooltime(actor, cooltime, slot),
			-- takeValue(actor, level, slot, slot, revision),
			-- takeValue(actor, level+1, slot, slot, revision),
		-- }
-- end


function staff_block(actor, level)
	local cooltime = blockCooltime
    local slot = 3
    local revision = 1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function staff_block_attack(actor, level)
	local cooltime = attackCooltime
    local slot = 8
    local revision = 1.35
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function staff_evade(actor, level)
	local cooltime = evadeAttackCooltime
    local slot = 4
    local revision = 0.25
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision) + level * 8,
			takeValue(actor, level+1, slot, slot, revision) + level * 15,
		}
end


function staff_interrupt_attack(actor, level)
	local cooltime = interruptAttackCooltime
    local slot = 5
    local revision = 1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

function staff_penetrate_attack(actor, level)
	local cooltime = inevitableAttackCooltime
    local slot = 9
    local revision = 3.72 / 3
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint01,
		}
end

function staff_inevitable_attack(actor, level)
	local cooltime = inevitableAttackCooltime
    local slot = 10
    local revision = 4.55
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint02,
		}
end

------------------------------------- spear ------------------------------------------------------------


function spear_attack_1(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.92 * 0.9					--local revision = 1.13 * 0.9
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function spear_attack_2(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.92 * 0.95					--local revision = 1.13
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function spear_attack_3(actor, level)
	local cooltime = attackCooltime
    local slot = 1
    local revision = 0.92 * 1.02					--local revision = 1.13 * 1.1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function spear_attack_4(actor, level)
	local cooltime = 3
    local slot = 1
    local revision = 0.92 * 1.11					--local revision = 1.13 * 1.3
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function spear_sub_attack(actor, level)
	local cooltime = 6
    local slot = 1
    local revision = 1.15					--local revision = 1.33 * 1.05
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function spear_strong_attack_1(actor, level)
	local cooltime = strongAttackCooltime
    local slot = 2
    local revision = 2.15 * 0.9					--local revision = 3.18 * 1.05
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function spear_strong_attack_2(actor, level)
	local cooltime = attackCooltime
    local slot = 2
    local revision = 2.15					--local revision = 3.58 * 1.05
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


function spear_block(actor, level)
	local cooltime = blockCooltime
    local slot = 3
    local revision = 1
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end


--[[function spear_evade(actor, level)
	local cooltime = evadeAttackCooltime
    local slot = 4
    local revision = 1.7
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end]]


function spear_evade(actor, level)
	local cooltime = evadeAttackCooltime
    local slot = 4
    local revision = 0.25
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision) + level * 8,
			takeValue(actor, level+1, slot, slot, revision) + level * 15,
		}
end


function spear_interrupt_attack(actor, level)
	local cooltime = interruptAttackCooltime
    local slot = 5
    local revision = 1.1					--local revision = 1.08 * 1.05
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
		}
end

function spear_penetrate_attack(actor, level)
	local cooltime = penetrateAttackCooltime
    local slot = 6
    local revision = 4.34					--local revision = 3.5 * 1.05
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint01,
		}
end


function spear_inevitable_attack(actor, level)
	local cooltime = inevitableAttackCooltime
    local slot = 7
    local revision = 8.93					--local revision = 5.78 * 1.05
	return
		{
			[0] = takeCooltime(actor, cooltime, slot),
            takeCooltime(actor, cooltime, slot),
			takeValue(actor, level, slot, slot, revision),
			takeValue(actor, level+1, slot, slot, revision),
			ragePoint02,
		}
end





