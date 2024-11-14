namespace StringMagician.Interfaces;

internal interface IUserInterface
{
	void WriteMessage(string message);
	string ReadInput();
}