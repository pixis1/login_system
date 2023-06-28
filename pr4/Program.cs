using Microsoft.VisualBasic.FileIO;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

FileInfo secretFile = new FileInfo(@"C:\Users\npk-s80-1\Documents\pop.txt");
FileInfo userFile = new FileInfo(@"C:\Users\npk-s80-1\Documents\users.txt");

Console.Write("Что собираетесь сделать?\nc - Создание пользователя\nEnter - Войти в систему\n");
switch (Console.ReadLine())
{
    case "c":
        create_user();
        break;
    case "":
        enter_system();
        break;
    default:
        Console.WriteLine("Вы ввели неправильное значение!");
        break;
}


void create_user()
{
    try
    {
        string buf0, buf1, buf2, buf3, buf4;

        Console.Write("Логин: ");
        buf0 = Console.ReadLine();
        if (!valid_login(buf0)) throw new Exception("Логин не правильный!");
        
        Console.Write("Пароль: ");
        buf1 = Console.ReadLine();
        buf1 = buf1.ToLower();
        if (!valid_password(buf1)) throw new Exception("Пароль не правильный!");

        Console.Write("ФИО: ");
        buf2 = Console.ReadLine();

        Console.Write("Город: ");
        buf3 = Console.ReadLine();

        Console.Write("Номер: ");
        buf4 = Console.ReadLine();

        File.AppendAllText(userFile.FullName, "login:" + buf0 + "\n");
        File.AppendAllText(userFile.FullName, "password:" + buf1 + "\n");
        File.AppendAllText(userFile.FullName, "nsp:" + buf2 + "\n");
        File.AppendAllText(userFile.FullName, "city:" + buf3 + "\n");
        File.AppendAllText(userFile.FullName, "number:" + buf4 + "\n");

    }
    catch (Exception e)
    {
        Console.WriteLine($"{e}");
    }
}
void enter_system()
{
    try
    {
        File.AppendAllText(userFile.FullName, "");
        string buf = File.ReadAllText(userFile.FullName);
        if (buf == "") throw new Exception("Нет пользователей. Создайте.");

        string password, login;
        Console.WriteLine("Логин: ");
        login = Console.ReadLine();

        string validPassword = find_login(login);
        Console.WriteLine("Пароль: ");
        password = Console.ReadLine();
        if (password != validPassword) throw new Exception("Пароль неверен. Логин верен.");

        Console.WriteLine("Вы вошли в систему!");
        Console.WriteLine("Что вы хотите сделать:");
        Console.WriteLine("c - сменить пароль");
        Console.WriteLine("s - доступ к секретной информации");
        switch (Console.ReadLine())
        {
            case "c":
                change_password(login);
                break;
            case "s":
                Console.WriteLine(return_secret);
                break;
        }
    }
    catch(Exception e)
    {
        Console.WriteLine(e);
    }


}

bool valid_password(string str)
{
    if (str.Length >= 6 && !Regex.IsMatch(str, @"\p{IsCyrillic}") && !Regex.IsMatch(str, @"\d"))
    {
        return true;
    }
    else return false;
}
bool valid_new_password(string str)
{
    int i = 0;
    while (i < str.Length)
    {
        if (str.IndexOf(str[i], i + 1) != -1)
        {
            return false;
        }
        i++;
    }
    return true;
}
bool valid_login(string str)
{
    if (!Regex.IsMatch(str, @"\p{IsCyrillic}") && !Regex.IsMatch(str, @"\d"))
    {
        return true;
    }
    else return false;
}
string find_login(string login)
{
    string buf = File.ReadAllText(userFile.FullName);
    int i = buf.IndexOf("login:" + login);
    if (i == -1) throw new Exception("Неверный логин, такого логина нет.");
    while (buf[i] != '\n')
    {
        i++;
    }
    while (buf[i] != ':')
    {
        i++;
    }
    i++;
    int a = i;
    while (buf[i] != '\n')
    {
        i++;
    }
    string password = buf.Substring(a, i - a);
    return password;
}
string return_secret()
{
    return File.ReadAllText(secretFile.FullName);
}
void change_password(string login)
{
    

    Console.WriteLine("Введите новый пароль: ");
    string newPassword= Console.ReadLine();
    newPassword = newPassword.ToLower();
    if(!valid_password(newPassword)) throw new Exception("Пароль не правильный, нужна латиница.");
    if (!valid_new_password(newPassword)) throw new Exception("Пароль не правильный, пароль не должен сожержать повторяющихся символом.");

    string buf = File.ReadAllText(userFile.FullName);
    int i = buf.IndexOf("login:" + login);
    i = buf.IndexOf("password:", i);
    int a = i;
    while (buf[a] != '\n')
    {
        a++;
    }
    buf = buf.Remove(i, a - i);
    buf = buf.Insert(i, "password:" + newPassword);
    File.WriteAllText(userFile.FullName, buf);
}