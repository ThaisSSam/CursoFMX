import SidebarComponent from '../components/Sidebar';

interface DashboardScreenProps {
  onLogout: () => Promise<void> | void;
}

export default function DashboardScreen({ onLogout }: DashboardScreenProps) {
  return (
    <div className="text-white bg-slate-900 min-h-screen flex flex-row">
      <SidebarComponent onLogout={onLogout} />
      <div className='flex flex-col w-full'>
        <header className="flex justify-between items-center border-b border-slate-700/70 pb-4 left-20 pt-5 pl-5 shadow-lg shadow-slate-700/40">
          <div>
            <h1 className="text-2xl font-bold">Dashboard</h1>
            <p className="text-sm text-slate-400">Seu sistema carregou com sucesso!</p>
          </div>
        </header>

        <main className="flex-1 bg-black/40">
          <div className='grid grid-cols-4 gap-3 p-5'>
            <div className='border border-slate-70/80 w-full rounded-lg p-2 bg-slate-900'>
              <p>Tarefas em aberto</p>
            </div>
            <div className='border border-slate-70/80 w-full rounded-lg p-2 bg-slate-900'>
              <p>Tarefas atrasadas</p>
            </div>
            <div className='border border-slate-70/80 w-full rounded-lg p-2 bg-slate-900'>
              <p>Tarefas concluídas hoje</p>
            </div>
            <div className='border border-slate-70/80 w-full rounded-lg p-2 bg-slate-900'>
              <p>Em andamento</p>
            </div>
          </div>
          <div className='grid grid-cols-2 gap-3 p-5'>
            <div className='border border-[#1f3454] w-full rounded-lg p-2 bg-slate-900'>
              <p>Tarefas por responsável</p>
            </div>
            <div className='border border-[#1f3454] w-full rounded-lg p-2 bg-slate-900'>
              <p>Tarefas por prioridade</p>
            </div>
          </div>
          <div className='grid grid-cols-1 gap-3 p-5'>
            <div className='w-full border border-[#1f3454] w-full rounded-lg p-2 bg-slate-900'>
              <p>Evolução semanal de conclusões</p>
            </div>
          </div>
          <p className="text-slate-300"></p>
        </main>
      </div >
    </div >
  );
}