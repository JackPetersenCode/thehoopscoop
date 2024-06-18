import os

def sayHello():
    return "helllloooooo"    

def test(message):
    directory = os.getcwd()
    return message + ': ' + directory

test("Testing.. Testing..")