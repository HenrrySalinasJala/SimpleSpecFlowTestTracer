Feature: SpecFlowFeature1

#Scenario: test 1 passed
#Given Step '1'
#When Step '2'
#Then Step '3'

#@bvt
#Scenario Outline: test 1 passed outline
#Given Given step '<next1>'
#When When step '<next2>'
#Then Then step '<next3>'
#Examples: 
#	| next1 | next2 | next3  |
#	| uno   | dos   | tres   |
#	| uno 2 | dos 2 | tres 2 |


#
#Scenario: test 2 passed
#Given test 1
#When Test 1
#Then Test 1
#
#
Scenario: test 3 failed
Given Given step '1'
Then Test failed
When When step '2'

Scenario: test 4 error
Given Given step '1'
Then Test error
When When step '2'

@Warning
Scenario: test 5 warning
Given Given step '1'
Then Test warning
#When When step '2'