local flag = 0

while true do
	local input = {}
	local joy = joypad.getimmediate()
	if mainmemory.read_u8(0xA002) == 0xFF then
		input["P1 R2"] = 1
	end
	if mainmemory.read_u8(0xA004) == 0xFF then
		input["P1 R1"] = 0
		input["P1 L1"] = 0
	end
	if mainmemory.read_u8(0xA006) == 0xFF then
		if joy["P1 Circle"] == 1 then
			input["P1 Circle"] = 0
		else
			input["P1 Circle"] = 1
		end
	end
	if mainmemory.read_u8(0xA008) == 0xFF then
		input["P1 Left"] = 0
	end
	if mainmemory.read_u8(0xA00A) == 0xFF then
		input["P1 Right"] = 0
	end
	joypad.set(input)
	emu.frameadvance()
end