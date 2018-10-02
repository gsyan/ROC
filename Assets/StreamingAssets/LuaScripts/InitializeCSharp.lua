-- C Sharp Types
luanet.load_assembly("System")
luanet.load_assembly("Common")

Console = luanet.import_type("System.Console")

-- Lua
math.randomseed(os.time())
