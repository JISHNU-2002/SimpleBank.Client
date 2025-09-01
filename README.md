# 🏛️ SimpleBank Client

The **SimpleBank Client** is an ASP.NET Core MVC application that acts as the frontend for the **SimpleBank API**.
It allows users to register, log in, manage bank accounts, perform transactions, and view account details through a clean web interface.

---

## 🚀 Features

* 🔑 **User Authentication & Authorization** (via JWT issued by API)
* 📝 **User Registration & Profile Update**
* 💳 **Account Management** (Account creation, Balance view)
* 💸 **Fund Transfers** between accounts
* 📜 **Transaction History** (Debit, Credit, Transfers)
* 🎨 **Dashboard View** displaying:

  * Account balance
  * User details
  * Recent transactions

---

## 🏗️ Tech Stack

* **ASP.NET Core MVC** – Client-side web application
* **Razor Views** – UI rendering
* **HttpClient** – API communication
* **Bootstrap / Tailwind** – Styling (choose based on what you used)
* **NToastNotify** – For toast notifications (if enabled)

---

## ⚙️ Setup Instructions

1. **Clone the repository**

   ```bash
   git clone https://github.com/your-username/SimpleBank.git
   cd SimpleBank/SimpleBank.Client
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Configure API Base URL**
   Open `appsettings.json` and set your **API URL**:

   ```json
    "ApiClientConfiguration": {
        "ClientUrl": "http://localhost:5217/"
     },
   ```

4. **Run the application**

   ```bash
   dotnet run
   ```

   The client will run on `https://localhost:5290` (default).

---

## 🔑 Authentication Flow

1. User logs in → Client sends credentials to **SimpleBank API**.
2. API responds with a **JWT Token**.
3. Token is stored in client-side session/cookies.
4. Token is attached to every request (via HttpClient).
5. API validates token → Returns secure data (accounts, transactions).

---

## Routes
- In **Models/ApiConstants** we set all the endpoints to api
```csharp
namespace SimpleBank.Client.Models
{
    public static class ApiConstants
    {
        // api/Authorize
        public const string AuthorizeUser = "api/Authorize/AuthorizeUser";
        public const string RegisterUser = "api/Authorize/RegisterUser";
        public const string ChangePassword = "api/Authorize/ChangePassword";

        // api/ApplicationForm
        public const string ApplicationForm = "api/ApplicationForm/AddForm";
        public const string GetAllForms = "api/ApplicationForm/GetAllForms";
        public const string ApproveForm = "api/ApplicationForm/ApproveForm";
        public const string RejectForm = "api/ApplicationForm/RejectForm";
        public const string FormDetails = "api/ApplicationForm/GetFormById";

        // api/Account
        public const string GetProfile = "api/Account/GetProfile";
        public const string UpdateProfile = "api/Account/UpdateProfile";
        public const string Dashboard = "api/Account/GetDashboard";
        public const string GetAllUsersWithDetails = "api/Account/GetAllUsersWithDetails";
        public const string GetUserById = "api/Account/GetUserById";
        public const string DeleteUserById = "api/Account/DeleteUserById";

        // api/Transactions
        public const string Transfer = "api/Transactions/Transfer";
        public const string Deposit = "api/Transactions/Deposit";
        public const string Withdraw = "api/Transactions/Withdraw";
        public const string GetAllTransactionsDetails = "api/Transactions/GetAllTransactionsDetails";

        // api/Branch
        public const string GetAllBranches = "api/Branch/GetAllBranches"; 
        public const string GetBranchByIFSC = "api/Branch/GetBranchByIFSC";
        public const string IFSCDetails = "api/Branch/IFSCDetails";
        public const string AddBranch = "api/Branch/AddBranch";
        public const string UpdateBranch = "api/Branch/UpdateBranch";
        public const string DeleteBranch = "api/Branch/DeleteBranch";

        // api/AccountType
        public const string GetAllAccountTypes = "api/AccountType/GetAllAccountTypes";
        public const string GetAccountTypeById = "api/AccountType/GetAccountTypeById";
        public const string AddAccountType = "api/AccountType/AddAccountType";
        public const string UpdateAccountType = "api/AccountType/UpdateAccountType";
        public const string DeleteAccountTypeById = "api/AccountType/DeleteAccountTypeById";

        // api/Role
        public const string GetAllRoles = "api/Role/GetAllRoles";
        public const string GetRoleById = "api/Role/GetRoleById";
        public const string AddRole = "api/Role/CreateRole";
        public const string RemoveRole = "api/Role/DeleteRole";
        public const string GetRoleDetailsById = "api/Role/GetRoleDetailsById";
        public const string UserRoles = "api/Role/UserRoles";
        public const string UpdateUserRoles = "api/Role/UpdateUserRoles";
    }
}
```

## GenericHttpClient

## 1. GetToken
The `GetToken` method is the **support function** your `GetAsync<T>` relies on. Let’s break it down step by step just like before:

---

### 📌 Function Signature

```csharp
private async Task<string> GetToken()
```

* `async Task<string>` → This is an **asynchronous** method that eventually returns a **string** (the JWT token).
* `private` → Only accessible inside the class (e.g., `ApiClient`).

---

### 1. Build Request

```csharp
var request = new HttpRequestMessage(HttpMethod.Post, "api/Token/GetToken");
```

* Creates an HTTP **POST request** to the API endpoint `api/Token/GetToken`.
* This is where the API expects credentials to issue a token.

---

### 2. Prepare Body Data

```csharp
Dictionary<string, string> dynamicRequest = new Dictionary<string, string>();
dynamicRequest.Add("username", "admin@gmail.com");
dynamicRequest.Add("password", "admin@123");
```

* A dictionary holds the login credentials.
* (Originally you were going to pull username/password from `appsettings.json` → `_configuration["ApiClientConfiguration:Username"]`).
* For now, it’s hardcoded.

- Better practice: use configuration or **secret manager** instead of hardcoding.

---

### 3. Attach Content

```csharp
var content = new StringContent(JsonConvert.SerializeObject(dynamicRequest));
request.Content = content;
request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
```

* Converts the dictionary into **JSON** (using Newtonsoft.Json).
* Wraps JSON in a `StringContent` object.
* Sets `Content-Type: application/json` header so the API knows it’s JSON.

Example payload sent to API:

```json
{
  "username": "admin@gmail.com",
  "password": "admin@123"
}
```

---

### 4. Send Request

```csharp
var response = await _client.SendAsync(request);
```

* Uses `_client` (an `HttpClient` instance) to send the request to API.
* Waits asynchronously for the response.

---

### 5. Check Response

```csharp
if(!response.IsSuccessStatusCode)
{
    throw new Exception("Failed to retrieve token from API.");
}
```

* If API returns anything other than `200 OK`, it throws an exception.

---

### 6. Read and Return Token

```csharp
var result = await response.Content.ReadAsStringAsync();
return result.ToString();
```

* Reads the raw response body as a string.
* Returns it as the token.

Example response from API:

```json
"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

That’s the JWT you’ll use in the `Authorization: Bearer <token>` header.

---

### 7. Final Fallback (Unreachable)

```csharp
throw new ApplicationException($"Token fetch failed. Status: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
```

* This is **unreachable code** right now because you already `return` inside the `else`.
* If you want to keep it, you should restructure the `if/else` so this line executes only when failure happens.

---

### ⚡ Flow Summary

1. Build a POST request with username/password in JSON.
2. Send request → `api/Token/GetToken`.
3. If API responds OK → extract JWT token from response.
4. Return token to caller (`GetAsync<T>`).
5. Use token in Authorization header for subsequent requests.

---

## 2. GetAsync

- In the case of http requests (get, post, put, delete) we write the generic function
Here’s the function for reference:

```csharp
public async Task<T> GetAsync<T>(string address)
{
    var request = new HttpRequestMessage(HttpMethod.Get, address);

    string token = await GetToken();
    _client.DefaultRequestHeaders.Authorization = 
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

    var response = await _client.SendAsync(request);
    if (!response.IsSuccessStatusCode)
    {
        throw new ApplicationException(
            $"Request failed. Status: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}"
        );
    }

    var result = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(result) 
           ?? throw new ApplicationException("Deserialization failed. Result is null.");
}
```

---

## 🔍 Detailed Explanation

### 1. Function Signature

```csharp
public async Task<T> GetAsync<T>(string address)
```

* `async Task<T>` → This method is asynchronous and returns a `Task<T>`, meaning it will eventually return a value of type `T`.
* `<T>` → This is a **generic type parameter**. It allows you to call the method with **any class or type** you expect from the response (e.g., `Account`, `Transaction`, `List<Account>`, etc.).


```csharp
var accounts = await apiClient.GetAsync<List<Account>>("api/accounts");
```

Here `T` becomes `List<Account>`.

---

### 2. Create Request

```csharp
var request = new HttpRequestMessage(HttpMethod.Get, address);
```

* Creates an HTTP GET request.
* `address` → The relative or absolute URL endpoint (e.g., `"api/dashboard/12345"`).
* `HttpRequestMessage` → Allows you to build and customize the request.

---

### 3. Add Authorization Header

```csharp
string token = await GetToken();
_client.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", token);
```

* Calls `GetToken()` → retrieves a **JWT access token**.
* Adds the header:

  ```
  Authorization: Bearer <token>
  ```
* Ensures the request is authenticated with the API.

---

### 4. Send Request

```csharp
var response = await _client.SendAsync(request);
```

* Uses the `_client` (an `HttpClient` instance) to actually send the request.
* `await` ensures you don’t block the thread while waiting for the server’s response.

---

### 5. Error Handling

```csharp
if (!response.IsSuccessStatusCode)
{
    throw new ApplicationException(
        $"Request failed. Status: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}"
    );
}
```

* `response.IsSuccessStatusCode` → checks if the response status code is **2xx (OK, Created, etc.)**.
* If **not successful**, throw an `ApplicationException` that includes:

  * The HTTP status (e.g., `401 Unauthorized`, `404 Not Found`, etc.).
  * The raw response content (helpful for debugging errors).

---

### 6. Read Response

```csharp
var result = await response.Content.ReadAsStringAsync();
```

* Reads the **response body** as a plain JSON string.
* Example:

  ```json
  {
    "accountNumber": "12345",
    "balance": 5000
  }
  ```

---

### 7. **Deserialize JSON → Object**

```csharp
return JsonConvert.DeserializeObject<T>(result) 
       ?? throw new ApplicationException("Deserialization failed. Result is null.");
```

* `JsonConvert.DeserializeObject<T>(result)` → Converts JSON into an object of type `T`.
* If deserialization fails and returns `null`, an exception is thrown.

```csharp
var account = await apiClient.GetAsync<Account>("api/accounts/12345");
```

If the API returns JSON like:

```json
{ "accountNumber": "12345", "balance": 5000 }
```

Then `account` will be an **Account object** populated with those values.

---

## ⚡ Why Use Generics Here?

* Reusable: Works with **any model** (`Account`, `Transaction`, `List<Transaction>`, etc.).
* Strongly-typed: You don’t need to cast or manually handle JSON — it returns the expected type.
* Clean code: One generic method instead of writing multiple `GetAccountAsync`, `GetTransactionsAsync`, etc.

---

## ✅ Example Usage

```csharp
// Get one account
var account = await apiClient.GetAsync<Account>("api/accounts/1001");

// Get all accounts
var accounts = await apiClient.GetAsync<List<Account>>("api/accounts");

// Get dashboard data
var dashboard = await apiClient.GetAsync<DashboardViewModel>("api/dashboard/1001");
```

---

# Sample Screens

![Home](https://github.com/JISHNU-2002/SimpleBank.Client/blob/master/SimpleBank.Client/Images/1_Home.png)

![Application Form](https://github.com/JISHNU-2002/SimpleBank.Client/blob/master/SimpleBank.Client/Images/2_NewAccount.png)

![Application Submit](https://github.com/JISHNU-2002/SimpleBank.Client/blob/master/SimpleBank.Client/Images/3_ApplicationSubmit.png)

![SuperAdmin](https://github.com/JISHNU-2002/SimpleBank.Client/blob/master/SimpleBank.Client/Images/4_SuperAdmin_Dashboard.png)

![Manager](https://github.com/JISHNU-2002/SimpleBank.Client/blob/master/SimpleBank.Client/Images/5_Manager_Dashboard.png)

![Customer](https://github.com/JISHNU-2002/SimpleBank.Client/blob/master/SimpleBank.Client/Images/6_Customer_Dashboard.png)
