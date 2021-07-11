result = [];
canUserSubmitSolution = True;
for test in tests:
	if (test[0] == test[1]):
		result.append(str(test[1]) + 'result:' + str(test[0]) + ' # True');
	else:
		result.append(str(test[1]) + 'result:' + str(test[0]) + ' # Frue');
		canUserSubmitSolution = False;
def getResult():
	return result;