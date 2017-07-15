using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using com.QH.QPGame.Services.Data;


public class DataConfig
{
    private DataTable m_dt = null;
    // 表头
    //private DataType m_data_type = new DataType();
    // 存AseetsBoundle配置数据
    private Dictionary<string, AssetBundleInfo> m_dic_data = new Dictionary<string, AssetBundleInfo>();
    private Dictionary<string, DataRow> m_dic_dataTable = new Dictionary<string, DataRow>();

    public DataTable dataTable
    {
        get { return m_dt; }
        set { m_dt = value; }
    }

    public Dictionary<string, AssetBundleInfo> DicAseetsData
    {
        get { return m_dic_data; }
    }

    public Dictionary<string, DataRow> DicDataTable
    {
        get { return m_dic_dataTable; }
    }

    public void AddDicAseetsData(AssetBundleInfo data_type, DataRow data_row)
    {
        if (!m_dic_data.ContainsKey(data_type.Name))
        {
            m_dic_data.Add(data_type.Name, data_type);
            m_dic_dataTable.Add(data_type.Name, data_row);
        }
        else
        {
            m_dic_data[data_type.Name] = data_type;
        }
    }

    public bool OpenFile(string path)
    {
        m_dt = CSVFileHelper.OpenCSV(path);
        if (m_dt == null)
        {
            return false;
        }

        FieldInfo[] optionFields = typeof (AssetBundleInfo).GetFields();
        foreach (DataRow dr in m_dt.Rows)
        {
            AssetBundleInfo data_temp = new AssetBundleInfo();
            foreach (var field in optionFields)
            {
                if (!field.IsPublic || field.IsStatic)
                {
                    continue;
                }

                field.SetValue(data_temp, dr[field.Name]);
            }

            AddDicAseetsData(data_temp, dr);
        }

        return true;
    }

    public void InitData(string path)
    {
        // 加载数据表
        if (!OpenFile(path))
        {
            Debug.Log("数据表为空");
            m_dt = new DataTable();
        }

        //TODO 采用反射
        FieldInfo[] optionFields = typeof (AssetBundleInfo).GetFields();
        foreach (var field in optionFields)
        {
            if (!field.IsPublic || field.IsStatic)
            {
                continue;
            }

            if (!m_dt.Columns.Contains(field.Name))
            {
                m_dt.Columns.Add(field.Name, field.FieldType);
            }
        }
    }

    public void AddRow(AssetBundleInfo data_type)
    {
        if (m_dic_data.ContainsKey(data_type.Name))
            return;

        DataRow dr = m_dt.NewRow();

        FieldInfo[] optionFields = typeof (AssetBundleInfo).GetFields();
        foreach (var field in optionFields)
        {
            if (!field.IsPublic || field.IsStatic)
            {
                continue;
            }

            dr[field.Name] = field.GetValue(data_type);
        }
        m_dt.Rows.Add(dr);

        AddDicAseetsData(data_type, dr);
    }


    public void Save(string path)
    {
        CSVFileHelper.SaveCSV(m_dt, path);
    }

    public void ExportToJson(string path)
    {
        ResVersionDesc desc = new ResVersionDesc();
        desc.AssetBundles = new List<AssetBundleInfo>();

        FieldInfo[] optionFields = typeof (AssetBundleInfo).GetFields();
        foreach (DataRow dr in m_dt.Rows)
        {
            var ab = new AssetBundleInfo();
            foreach (var field in optionFields)
            {
                if (!field.IsPublic || field.IsStatic)
                {
                    continue;
                }

                field.SetValue(ab, dr[field.Name]);
            }

            desc.AssetBundles.Add(ab);
        }

        string text = LitJson.JsonMapper.ToJson(desc);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        File.WriteAllText(path, text);
    }
}
