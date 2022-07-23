namespace FrontToBack.Helper
{
    public static class Helper
    {
        public static void DeleteImage(string path) 
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public enum UserRoles
        { 
        Admin,
        Memmber,
        SuperAdmin
        }
    }
}
