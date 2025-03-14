﻿namespace eCommerceDs.Services
{
    public interface IeCommerceDsService<T, TI, TU>
    {
        public List<string> Errors { get; }
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int id);
        Task<T> Add(TI tInsertDTO);
        Task<T> Update(int id, TU tUpdateDTO);
        Task<T> Delete(int id);
        bool Validate(TI dto);
        bool Validate(TU dto);

    }
}
