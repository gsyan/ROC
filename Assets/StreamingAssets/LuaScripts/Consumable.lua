function consumable_attack(actor)
	actor.attackPower = math.floor(actor.attackPower + actor.attackPower * 0.15)
	actor.penetration = math.floor(actor.penetration + actor.penetration * 0.15)
	actor.critical = math.floor(actor.critical + actor.critical * 0.15)
	actor.accuracy = math.floor(actor.accuracy + actor.accuracy * 0.15)
end


function consumable_defence(actor)
	actor.defence = math.floor(actor.defence + actor.defence * 0.15)
	actor.criticalDefence = math.floor(actor.criticalDefence + actor.criticalDefence * 0.15)
	actor.evasion = math.floor(actor.evasion + actor.evasion * 0.15)
	actor.maxHp = math.floor(actor.maxHp + actor.maxHp * 0.15)
	
	actor.hp = actor.maxHp;
end


function consumable_healing(actor)
	actor.hp = math.min(actor.maxHp, math.floor(actor.hp + actor.maxHp * 0.2))
end

