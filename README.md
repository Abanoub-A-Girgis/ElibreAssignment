# Elibre Assessment 2022

Project Details:
You are required to develop a simple desktop application that will be used to draw very basic 
architectural floor plans. The following user stories should be available:
## User Story (1):
As I user, I can left-click anywhere within the designated drawing space to draw lines. These 
lines represent walls of a floor plan.
Testing Criteria:
1- Left click on drawing space to start drawing a wall line,
2- Line gradually gets created as I move my cursor left or right. Note: movement can only
be in the direction of the x & y axis (orthogonal lines only),
3- Once I have reached the desired length for my wall line, I left-click again to stop drawing
set line.
4- If I left click again, a new line gets initiated and points 2&3 are then to follow.
##User Story (2):
As a user if I click on any point along a wall line, a door gets created.
Testing Criteria:
1- Left-click on wall line, a door symbol is placed at that point.
##User Story (3):
As a user if I click on a door, the door is changed to a window.
Testing Criteria:
1- Left-click on a door, the door symbol is changed to a window symbol.
##User Story (4):
As a user, I can delete any component created. (i.e.: wall line, door, window