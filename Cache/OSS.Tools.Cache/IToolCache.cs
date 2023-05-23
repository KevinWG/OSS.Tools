#region Copyright (C) 2016 Kevin (OSS��Դϵ��) ���ںţ�OSSCore

/***************************************************************************
*����	�ļ�����������ȫ�ֲ�� -  �������ӿ�
*
*����	�����ˣ� Kevin
*       ������Email��1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion


namespace OSS.Tools.Cache
{
    /// <summary>
    /// �������ӿ�
    /// </summary>
    public interface IToolCache//: IMemoryCache
    {
        /// <summary>
        /// ��ӹ̶�����ʱ�仺��,������������
        /// </summary>
        /// <typeparam name="T">��ӻ����������</typeparam>
        /// <param name="key">��Ӷ����key</param>
        /// <param name="obj">ֵ</param>
        /// <param name="cacheOpt"></param>
        /// <returns>�Ƿ���ӳɹ�</returns>
        Task<bool> SetAsync<T>(string key, T obj, CacheTimeOptions cacheOpt);

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <typeparam name="T">��ȡ�����������</typeparam>
        /// <param name="key">key</param>
        /// <returns>��ȡָ��key��Ӧ��ֵ </returns>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// �Ƴ��������
        /// </summary>
        /// <param name="key"></param>
        /// <returns>�Ƿ�ɹ�</returns>
        Task<bool> RemoveAsync(params string[] key);
    }
}