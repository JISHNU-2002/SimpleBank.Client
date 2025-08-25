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
