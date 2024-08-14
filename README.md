<a name="readme-top"></a>


<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


<!-- PROJECT LOGO -->
<div align="center">
  <h3 align="center">Tick</h3>
  <p align="center">
    Minimalist to-do list API
    <br />
    <br />
    <a href="https://github.com/chinweibegbu/tick-backend">Explore Docs</a>
    .
<!--     <a href="https://www.youtube.com/channel/UCRWhX1g2ADZKLWMflBtVNxQ">View Demo</a>
    · -->
    <a href="https://github.com/chinweibegbu/tick-backend/issues">Report Bug</a>
    ·
    <a href="https://github.com/chinweibegbu/tick-backend/issues">Request Feature</a>
  </p>  
</div>


<!-- TABLE OF CONTENT -->
<details>
  <summary>Table of Content</summary>
  <ul>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#overview">Overview</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#features">Features</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ul>
</details>


<!-- ABOUT THE PROJECT -->
## About The Project

### Overview

This is the API for Tick, a minimalist to-do list application

<p align="right">(<a href="#readme-top">back to top</a>)</p>


### Built With

* [![C#][C#]][C#-url]
* [![ASP.NET Core][ASP.NET Core]][ASP.NET Core-url]
* [![JSON Web Tokens][JSON Web Tokens]][JSON Web Tokens-url]
* [![Microsoft SQL Server][Microsoft SQL Server]][Microsoft SQL Server-url]
* ![AWS][AWS]
  * [AWS RDS](https://aws.amazon.com/rds/)
  * [AWS Elastic Beanstalk](https://aws.amazon.com/elasticbeanstalk/)
  * [AWS CloudFront](https://aws.amazon.com/cloudfront/)
* ![Visual Studio][Visual Studio]
  * [AWS Toolkit for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=AmazonWebServices.AWSToolkitforVisualStudio2022)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running, follow these simple steps.

### Prerequisites

* .NET 8.0 SDK
* Visual Studio 2022

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/chinweibegbu/tick-backend.git
   ```
2. Install NuGet packages
3. Start local environment by clicking on the "IIS Express" debug button

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTRIBUTING -->
## Contributing

If you have a suggestion that would make this better, follow these steps:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/NewFeature`)
3. Commit your Changes (`git commit -m 'Add some NewFeature'`)
4. Push to the Branch (`git push origin feature/NewFeature`)
5. Open a Pull Request

Alternatively, you can open an [Issue](https://github.com/chinweibegbu/tick-backend/issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- FEATURES -->
## Features

### General
- [x] `/info`
- [x] `/health`

### User Management
- [x] `/api/User/authenticate`
- [x] `/api/User/logout`
- [x] `/api/User/getUsers`
- [x] `/api/User/getUser/{id}`
- [x] `/api/User/addUser`
  - [x] userName
  - [x] firstName
  - [x] lastName
  - [x] password
  - [x] email
  - [ ] profilePhoto
- [x] `/api/User/editUser`
- [x] `/api/User/deleteUser`
- [ ] `/api/User/resetUser`
- [x] `/api/User/resetUserLockout`
- [ ] `/api/User/passwordReset`

### Task Management
- [x] `/api/Task/getTasks`
- [x] `/api/Task/getTasksByUserId`
  - [ ] Filter completed tasks
  - [ ] Filter uncompleted tasks
- [x] `/api/Task/addTask`
- [ ] `/api/Task/duplicateTask/{taskId}`
- [x] `/api/Task/editTask/{taskId}`
- [x] `/api/Task/toggleCompleteTask/{taskId}`
- [x] `/api/Task/deleteTask/{taskId}`

See the [open issues](https://github.com/chinweibegbu/tick-backend/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTACT -->
## Contact

Chinwe Ibegbu - chinwe.ibegbu@gmail.com

Project Link: [https://github.com/chinweibegbu/tick-backend/](https://github.com/chinweibegbu/tick-backend/)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [README.md template by Drew Othneil](https://github.com/othneildrew/Best-README-Template)
* [Awesome Badges by Vedant Chainani](https://dev.to/envoy_/150-badges-for-github-pnk)
* [Markdown Badges by Ileriayo Adebiyi](https://github.com/Ileriayo/markdown-badges)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/chinweibegbu/tick-backend.svg?style=for-the-badge
[contributors-url]: https://github.com/chinweibegbu/tick-backend/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/chinweibegbu/tick-backend.svg?style=for-the-badge
[forks-url]: https://github.com/chinweibegbu/tick-backend/network/members
[stars-shield]: https://img.shields.io/github/stars/chinweibegbu/tick-backend.svg?style=for-the-badge
[stars-url]: https://github.com/chinweibegbu/tick-backend/stargazers
[issues-shield]: https://img.shields.io/github/issues/chinweibegbu/tick-backend.svg?style=for-the-badge
[issues-url]: https://github.com/chinweibegbu/tick-backend/issues
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/chinwe-ibegbu
[product-screenshot]: https://drive.google.com/file/d/162i407l3yuw7hCjHVDJ8t_wPiVA16cnZ/view?usp=sharing

[C#]: https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white
[C#-url]: https://learn.microsoft.com/en-us/dotnet/csharp/
[ASP.NET Core]: https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[ASP.NET Core-url]: https://dotnet.microsoft.com/en-us/apps/aspnet
[JSON Web Tokens]: https://img.shields.io/badge/json%20web%20tokens-323330?style=for-the-badge&logo=json-web-tokens&logoColor=pink
[JSON Web Tokens-url]: https://jwt.io/
[Microsoft SQL Server]: https://img.shields.io/badge/Microsoft_SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white
[Microsoft SQL Server-url]: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
[AWS]: https://img.shields.io/badge/Amazon_AWS-FF9900?style=for-the-badge&logo=amazonaws&logoColor=white
[Visual Studio]: https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual%20studio&logoColor=white
