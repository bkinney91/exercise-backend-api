# Project Description

This project is about exposing a single web api endpoint that returns the most recent post authored by each user from a third party data set.

## Requirements
- You can choose ASP.NET or .NET Core, please let us know if you choose .NET core so we can change the environment on azure so auto deploys will work as you push code to the master branch.

- Call your project Fieldlevel so that end up with a FieldLevel.csproj
- The data set is located here https://jsonplaceholder.typicode.com/posts
- Results should display the most recent post authored by each user. Use post.id to determine recency.
- Regardless of the frequency at which your endpoint is requested across multi devices, the third party data set should not be requested more than once per minute.
- Due to the simplicity of this test, you'll also be evaluated based on thoroughness and thoughtfulness of your solution. 
- Code the exercise as if you were doing this at a job.
- Please do not spend more than a 4 hours on this task.

## Deliverables

- Please provide a single solution file (\*.sln) that can be compiled and launched from Visual Studio.
- import the exercise repository to your personal Github account using [github's import tool](https://github.com/new/import) import this url: https://github.com/FieldLevel/exercise-backend-api
- complete the exercise
- invite our github user "fl-codereview" to be a collaborator on the repository
- create a pull request against your Github repository
- let us(your interview coordinator) know when you are ready to review


# How To Run
Using VS:
    Run the solution using "IIS Express Production" for the app to connect to the data source provided
Using CLI
    Open a terminal at the FieldLevel.csproj directory and run "dotnet run"
    Open a browser and navigate to https://localhost:5001/swagger/index.html

# Deployment
This will be auto deployed to an azure site
https://{git-repo}.azurewebsites.net

Customizing
https://github.com/projectkudu/kudu/wiki/Customizing-deployments

# FAQ

Q: **Can I ask questions?**

A: Of Course. Often, the answer will be "You choose".
