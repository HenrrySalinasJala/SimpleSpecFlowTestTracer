@NewFeature
Feature: Test Engine
As a user
I want a new feature
So i can do new stuff

@BVT
Scenario: test with table
Given Given step '<next1>'
When When step '<next2>'
Then Then step '<next3>'
Then Step with table
| column1 | column2 |
| Xy      | IO      |
| Aw      | Bt      |


@BVT
Scenario: test with transformed table
Given Given step '<next1>'
When When step '<next2>'
Then Then step '<next3>'
Then Step with transformed
| column1 | TRANSFORMED |
| Xy      | IO      |
| Aw      | Bt      |


@BVT
Scenario: test with Multiline text
Given Given step '<next1>'
When When step '<next2>'
Then Then step '<next3>'
Then Step with multiline text
"""
Text in multiline
is easy to use
"""

