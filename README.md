# eCommerceService
The objective of an e-commerce web API is to enhance the functionality, flexibility, and accessibility of an e-commerce platform, ultimately improving the user experience, enabling business growth, and facilitating integration with a wide range of services and systems.

This Web API includes operations for:
- Orders management
- Customer master data management and customer search
- Products information management and product search
- Product inventory management

In this project, eCommerce Web API is developed which can be used for online shopping
- It offers several benefits that enhance functionality, efficiency, and user experience
- It streamlines operations and provides a seamless experience for both customers and administrators.
- It is completely decoupled with the front-end and it enables faster time to market
- It promotes interoperability and integration across heterogeneous environments regardless of programming languages, platforms and technologies
- It is modular, reusable and can be scaled independently.

# Getting Started
## Add your files

- [ ] [Create](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#create-a-file) or [upload](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#upload-a-file) files
- [ ] [Add files using the command line](https://docs.gitlab.com/ee/gitlab-basics/add-file.html#add-a-file-using-the-command-line) or push an existing Git repository with the following command:

```
cd existing_repo
git remote add origin https://gitlab.com/nehapardeshi/eCommerceService.git
git branch -M main
git push -uf origin main
```

## Setup Project
- Ensure to have connection string of SQL Server where new eCommerce database can be created
- Clone repo and open eCommerceService.sln solution file
- Update ConnectionStrings.SqlConnection value in the appsettings.json file
- Build the solution
- Open package manage console and run Update-Database
- Run the Web API and swagger UI will open

## Docker setup
- Use dockerfile to build and run the container image

# Future Scope
- User registration and authentication
- User account management
- Inventory Out of stock notifications
- Low stock alerts
- Address validation & shipment tracking
- Return/Refund handling of the orders
- Applying discounts and coupons
- Customer review and ratings
- Adding nice front-end web application that consumes eCommerce Web API for better user experience