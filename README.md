# Coding challenge 01

## Introduction

Both fertilizer and the environment are very precious and it is wise to keep these in harmony.

Your goal is to calculate how effective some fertilizer spreading has been according to what was originally planned in a map by the farmer versus what was actually applied by the fancy spreading robot.

The results of this can then be used to monitor and highlight future spreading precision improvements to the robot.

---

## Instructions

You will need to complete a console application which accepts it's inputs via a commandline argument and outputs the results into standard output (console).

The starting template for your console application is located under the `csharp` folder.


### Input

You will receive a single input string argument which contains 3 sections separated by the pipe `|` character:
- 2D square map evenly divided into cells with rows and columns. The map shows the farmers plan and whether or not each cell is expected to have either 0 or 1 units of fertilizer spread on it
- the starting amount of fertilizer units the spreading robot has
- the remaining amount of fertilizer units the spreading robot has when leaving the cell (2 digits)

Within each of the sections the semicolon `;` character signifies an end of the row.

#### Example Input: 

`0111;0101;1111;0011|12|12111009;09090906;06060504;04040302` represents the following:

Planned spreading map:
|||||
|-|-|-|-|
|0|1|1|1|
|0|1|0|1|
|1|1|1|1|
|0|0|1|1|

Remaining fertilizer units on spreading robot:
|||||
|-|-|-|-|
|12|11|10|9|
|9|9|9|6|
|6|6|5|4|
|4|4|3|2|


### Output

You are expected to use the input data to calculate and return the following results in a single string separated with the pipe `|` character:
- total count of incorrectly spread cells (either over spread or under spread)
- total count of over spread cells
- total count of under spread cells
- percentage of the cells spread correctly (rounded up to the nearest whole number)

### Example Output:
From the above Example Input, we would expect the output to be: `4|1|3|75`

- Incorrectly spread cells = 4
- Over spread cells = 1
- Under spread cells = 3
- Percentage accuracy % = 75

## Assumptions

The 2D map will always be square in shape and will not ever have more than 9 rows or columns of data.
The input data is presented in a left-to-right and top-to-bottom direction.

---

## Submissions

You will submit your solution as a Pull Request to this repository and then request a review on it.

