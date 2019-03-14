
function GetDefenceRate(defence)
	local a = 500
	local b = 0
	local c = 10000
	local d = 50 --최소값
	--bk, defence 0~1111 이면 rate 50 ~ 99.99, 참고: 100을 넘어도 유효함
	--print("GetDefenceRate: "..a * (defence + b) / (defence + c) + d)
	return a * (defence + b) / (defence + c) + d 
end

function GetPenetrationRate(penetration)
	local a = 500
	local b = 0
	local c = 10000
	local d = 0 --최소값
	--bk, penetration 0~2500 이면 rate 0 ~ 100, 참고: 100을 넘어도 유효함
	--print("GetPenetrationRate: "..a * (penetration + b) / (penetration + c) + d)
	return a * (penetration + b) / (penetration + c) + d
end

function GetAccuracyRate(accuracy)
	local a = 500
	local b = 0
	local c = 10000
	local d = 80 --최소값
	--bk, accuracy 0~416 이면 rate 80 ~ 99.969, 참고: 100을 넘어도 유효함
	--print("GetAccuracyRate: "..a * (accuracy + b) / (accuracy + c) + d)
	return a * (accuracy + b) / (accuracy + c) + d
end

function GetEvasionRate(evasion)
	local a = 500
	local b = 0
	local c = 10000
	local d = 0 --최소값
	--bk, evasion 0~2500 이면 rate 0 ~ 100, 참고: 100을 넘어도 유효함
	return a * (evasion + b) / (evasion + c) + d
end

function GetCriticalRate(critical)
	local a = 500
	local b = 0
	local c = 10000
	local d = 20 --최소값
	--bk, critical 20~1904 이면 rate 20 ~ 99.97, 참고: 100을 넘어도 유효함
	return a * (critical + b) / (critical + c) + d
end

function GetCriticalEvasionRate(criticalDefence)
	local a = 500
	local b = 0
	local c = 10000
	local d = 0 --최소값
	--bk, criticalDefence 0~2500 이면 rate 0 ~ 100, 참고: 100을 넘어도 유효함
	return a * (criticalDefence + b) / (criticalDefence + c) + d
end


function CalculatePvpStats(actor)
	actor.maxHp = actor.maxHp * 0.1 + 53814
	actor.hp = actor.hp * 0.1 + 53814
	actor.attackPower = actor.attackPower * 0.1 + 2691
	actor.defence = actor.defence * 0.1 + 6419
	actor.penetration = actor.penetration * 0.1 + 6419
	actor.accuracy = actor.accuracy * 0.1 + 6419
	actor.evasion = actor.evasion * 0.1 + 6419
	actor.critical = actor.critical * 0.1 + 6419
	actor.criticalDefence = actor.criticalDefence * 0.1 + 6419
end


function CalculatePvpAIStats(actor)
	actor.maxHp = actor.maxHp * 0.1 + 53814
	actor.hp = actor.hp * 0.1 + 53814
	actor.attackPower = actor.attackPower * 0.1 + 2691
	actor.defence = actor.defence * 0.1 + 6419
	actor.penetration = actor.penetration * 0.1 + 6419
	actor.accuracy = actor.accuracy * 0.1 + 6419
	actor.evasion = actor.evasion * 0.1 + 6419
	actor.critical = actor.critical * 0.1 + 6419
	actor.criticalDefence = actor.criticalDefence * 0.1 + 6419
end


function CalculateDailyDungeonStats(stats)
	stats.attackPower = stats.attackPower * 0.5 + 500
	stats.defence = stats.defence * 0.5 + 1000
	stats.penetration = stats.penetration * 0.5 + 1000
	stats.accuracy = stats.accuracy * 0.5 + 1000
	stats.evasion = stats.evasion * 0.5 + 1000
	stats.critical = stats.critical * 0.5 + 1000
	stats.criticalDefence = stats.criticalDefence * 0.5 + 1000
	stats.hp = stats.hp * 0.5 + 5000
end


function BattlePower(actor)
	local bp, pr, ar, cr
	
	pr = GetPenetrationRate(actor.penetration)
	ar = GetAccuracyRate(actor.accuracy)
	cr = GetCriticalRate(actor.critical)
	
	bp = math.floor(actor.attackPower * (1.1^(pr/10)) * (1.1^((ar*0.5)/10)) * (1.1^((cr*0.5)/10)))
	
	return bp 
end


function SurvivalPower(actor)
	local sp, dr, er, cdr
	
	dr = GetDefenceRate(actor.defence)
	er = GetEvasionRate(actor.evasion)
	cdr = GetCriticalEvasionRate(actor.criticalDefence)
	
	sp = math.floor((actor.maxHp/10) * (1.1^(dr/10)) * (1.1^((er*0.5)/10)) * (1.1^((cdr*0.5)/10)))
	
	return sp
end