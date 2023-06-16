# Can you C#
This was a great learning experience to consume another API. If you are unable to see the hashtag symbol in any of the namespaces then you, unidentified person, cannot C#.
![Lovely](https://wompampsupport.azureedge.net/fetchimage?siteId=7575&v=2&jpgQuality=100&width=700&url=https%3A%2F%2Fi.kym-cdn.com%2Fentries%2Ficons%2Ffacebook%2F000%2F021%2F807%2Fig9OoyenpxqdCQyABmOQBZDI0duHk2QZZmWg2Hxd4ro.jpg)
P.S. I've left some notes in the code highlighting my thought process

[[_TOC_]]

## Setup
### Method 1
1. Open Visual Studio 2022
2. File > Open Project
3. Navigate to and open the .csproj of this project
4. Click the play button
5. Mess with what I've done 

### Method 2
1. Download and install Postman
2. Open Postman
3. Create new GET request and enter the following URL for the request
```
https://localhost:1337/GetNoticeLeaseSchedulesFromRawScheduleData
```
4. Click Authorization tab and select Basic Auth
5. Enter the following credentials
```
Username: CanYou
Password: SeeTheSharp
```
6. Click Send
7. Relish in the glory of the response

## Improvements
- [ ] Add more error handling
- [x] Add interfaces
- [x] Add authentication
- [ ] Add config file where I'd store (I want to emphasise that I would have done this if this wasn't a test)
	- [ ] API URL
	- [ ] Username
	- [ ] Password
- [ ] Add unit tests
- [ ] Add logging
- [ ] Add Swagger documentation
