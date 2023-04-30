# TomaNota ~ Milan Murray X00162027

## Setup Steps

### Web Client (Deployed)

URL: https://tomanota2023mm.azurewebsites.net/

### Web Client (Local)

``` shell
cd code/web-client/TomaNota/
dotnet run
```

### RESTful Web API (Local)

``` shell
cd code/user-web-API/
dotnet run
```

## About

TomaNota is a note learning application aimed at languages that helps memorise the notes that we take using spaced repetition. 

### Scope

This project is aimed at a wide range of users and focuses on the desktop environment where users have both increased control over their documents as well as more time to dedicate to a study session. As the application makes use of cloud functions as well as a RESTful api, these existing systems can be reused to create new clients such as the android client.

### Functional requirements

TomaNota's functional requirements consist of being able to create a list of titles and definitions from a correctly formatted document. These prompts must then be shown to the user to assess the difficulty of recalling the definition. Each prompt must be rated on its difficulty such that harder prompts will occur more often. Results of each session are to be stored in a way that relates to the original document allowing multiple documents to be used in a simple plug-and-play fashion. To accommodate a variety of users of the software, it will also contain a structured collection of notes that can be used as an example of the documents the user can create themselves, or to simply use the provided material without needing to make notes.

### Non-functional Requirements

The application’s non-functional requirements are to be intuitive and easy to use. Best practices and methods such as the Gestalt laws are applied in such a way that users of the application struggle less when interacting with the interface, as no time should be wasted throughout their study session if avoidable.

### Future options

- Additional advanced features such as voice synthesis or machine learning may also add more opportunities for expanding the functionality of the software and increase options for learning methods.
- Admin interface to manage content of the application as well as the database.

### Language support plans

The software is also aimed to be created allowing for the interface to be displayed in multiple languages, allowing for increased accessibility to more people.
