import os

def sayHello():
    return "helllloooooo lowercase"    

def test(message):
    directory = os.getcwd()
    return message + ': ' + directory

test("testing")