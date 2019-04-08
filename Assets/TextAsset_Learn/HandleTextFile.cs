using UnityEngine;
using UnityEditor;
using System.IO;
public class HandleTextFile
{
	[MenuItem("Tools/Multiplication table")]
	static void WriteOneToTenMultiply()
	{
		string path = "Assets/Resources/test.txt";
		StreamWriter writer = new StreamWriter(path, true);
		
		writer.WriteLine( "Generated table of 1 to 10" );
        writer.WriteLine("");

        for ( int i = 1; i <= 10; i++ )
        {
            for ( int j = 1; j <= 10; j++ )
            {
                writer.WriteLine( "{0}x{1}= {2}", i, j, (i*j) );
            }
        }

        Debug.Log( "Table successfully written to file!" );
        writer.Close();
	}
	[MenuItem("Tools/ClearFile")]
	static public void ClearTextFile()
	{
		string path = "Assets/Resources/test.txt";
		StreamWriter writer = new StreamWriter(path, false);

		writer.Write("");
		Debug.Log("Clear text file");
		writer.Close();
	}
}
