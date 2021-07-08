result = [];
canUserSubmitSolution = True;
for test in tests:
	if (test[0] == test[1]):
		result.append(str(test) + 'result: True');
	else:
		result.append("str(test) + 'result: False'");
		canUserSubmitSolution = False;
def getResult():
	return result;