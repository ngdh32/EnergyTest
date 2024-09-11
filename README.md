Settings:


Considerations:

Process it asynchonously;
If returning the number is not necessary, what we can do is create an endpoint in AWS Api Gateway, associate it with a lambda that does the validation checkin. If the validation passes, store it in S3 bucket with life policy that move the file to S3 intelligent tier or IA tier, use Amazon EventBridge to trigger another lambda when a file is uploaded and then process it with the action in this solution. That's why I move the action to core so that the new lambda project can reference it without referencing the API.  

Database connection:
Put it in the AWS secret manager. If we are using AWS RDS with read replica enabled, we can have 2 different connection string. One is for read/write and one is for read only so as to ease the loading. 

In MeterReadingUploadsAction.cs, may consider having in memroy dictionary so that we dont have to access to the database every line

Questions:
1. What is the x in the line 29 in the Meter_Reading.csv? Why is there a missing column?
2. If some of the entries are correct and some of them are not, should those correct entries continue to be processed? (Assumption: Yes)