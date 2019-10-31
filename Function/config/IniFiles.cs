using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class IniFiles : MonoBehaviour


{
    public string inipath;

    //声明API函数

    /// <summary>
    /// 写操作
    /// </summary>
    /// <param name="section">节</param>
    /// <param name="key">键</param>
    /// <param name="val">值</param>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    /// <summary>
    /// 读操作
    /// </summary>
    /// <param name="section">节</param>
    /// <param name="key">键</param>
    /// <param name="def">未读取到的默认值</param>
    /// <param name="retVal">读取到的值</param>
    /// <param name="size">大小</param>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    /// <summary> 
    /// 构造方法 
    /// </summary> 
    /// <param name="INIPath">文件路径</param> 
    
    public IniFiles() { }
    public IniFiles(string INIPath)
    {
        inipath = INIPath;
    }


    /// <summary> 
    /// 写入INI文件 
    /// </summary> 
    /// <param name="Section">项目名称(如 [TypeName] )</param> 
    /// <param name="Key">键</param> 
    /// <param name="Value">值</param> 
    public void IniWriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, this.inipath);
    }

    /// <summary> 
    /// 读出INI文件 
    /// </summary> 
    /// <param name="Section">项目名称(如 [TypeName] )</param> 
    /// <param name="Key">键</param> 
    public string IniReadValue(string Section, string Key)
    {
        StringBuilder temp = new StringBuilder(500);
        int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
        return temp.ToString();
    }


    /// <summary> 
    /// 验证文件是否存在 
    /// </summary> 
    /// <returns>布尔值</returns> 
    public bool ExistINIFile()
    {
        return File.Exists(inipath);
    }
    private void Awake()
    {
        
    }
}