luanet.load_assembly("UnityEngine") 
luanet.load_assembly("Assembly-CSharp")

Application = luanet.import_type("UnityEngine.Application")

math.randomseed(os.time())
