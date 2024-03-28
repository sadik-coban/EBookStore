# EBookStore
In this project I used mailtrap for smtp server. If you don't want to use mailtrap you can directly use customer credentials below.

Email = user@mail.com,
PasswordForDevelopment = 1,
PasswordForProduction = !EBookStore1234

For Admin User: 
Email = "manager@ebookstore.com",
PasswordForDevelopment = 1,
PasswordForProduction = !EBookStore1234

For Mailtrap modify appsettings.json
  "Email": {
    "Server": "sandbox.smtp.mailtrap.io",
    "Port": 2525,
    "SenderName": "EShop",
    "SenderEmail": "0ac1675cd2fafe",
    "Account": "e2de724b62f294",
    "Password": "bfb706e46a2439",
    "SSL": true
  },
  other settings in appsettings.json and appsetting.Development.json too

This is my final project for my course. I used 3-tier architecture. Core is the layer for infrastructure persistence domain and response entites,
Bussiness layer responsible for application flow, Controller and Razor engines layer (Web layer) responsible for controlling url, website inputs etc.
