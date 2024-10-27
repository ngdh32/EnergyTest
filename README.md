# Related AWS Resource:
- EC2/Fargate
- Load Balancer/Api Gateway
- Route 53 
- VPC
- AWS WAF for extra web security

# Suggestions:
- Use the read replica to get back the account from the database
- Use the redis to host the list of the account for better performance
- Use CDK program/terraform to deploy aws resources
- Use github actions to streamline CI/CD
- Use Parallel Reading if necessary
- In DB, create an index on table MeterReading(AccountId, ReadingTime) to boost the performance

# Pitfall:
- If the file size is too big
- If there are too many requests

# Alternative Production Architecture:
1. Create a Api Gateway with lambda that helps save the uploaded file to S3 bucket
2. Create S3 Upload Event to trigger another lambda to process the file
3. Do the validation and save the data to the database in the lambda
4. The maximum time for the lambda is 15 minutes. If the file size is too large, consider convert it to the fargate task
5. Create a S3 policy to move the files to S3 Glacier if the files need to be kept for compliance reason
6. If returning a result is really necessary, then create a SignalR hub to send back the result once the work in the background is done 

# Assumptions:
1. What is the "x" in the line 29 in the Meter_Reading.csv? Why is there a missing column? (Assumption: it is a deleted column or remark column)
2. If some of the entries are correct and some of them are not, should those correct entries continue to be processed? (Assumption: Yes)
3. Does the number format NNNNN not support negative nummbers e.g. -6575? (Assumption: still process so the total meter value can be deducted for adjustment reason)
4. Assume the entries are sorted by the created date in the asc order