namespace PYPA.Transacoes.DataMapping
{
    public interface IContaLockRepository
    {
        bool GetAndSetLock(long numeroConta);

        bool ReleaseLock(long numeroConta);
    }
}