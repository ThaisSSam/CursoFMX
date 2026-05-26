import React from 'react';
import { useNavigate } from 'react-router-dom';

export default function DashboardScreen() {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('auth_user');

    navigate('/login', { replace: true });
  };

  return (
    <div className="p-8 text-white bg-slate-900 min-h-screen flex flex-col">
      <header className="flex justify-between items-center border-b border-slate-800 pb-4 mb-6">
        <div>
          <h1 className="text-2xl font-bold">Dashboard</h1>
          <p className="text-sm text-slate-400">Seu sistema carregou com sucesso!</p>
        </div>

        <button
          onClick={handleLogout}
          className="bg-red-600/20 hover:bg-red-600 text-red-400 hover:text-white px-4 py-2 rounded-lg text-sm font-medium transition-colors duration-200 border border-red-500/30"
        >
          Sair do Sistema
        </button>
      </header>

      <main className="flex-1">
        <p className="text-slate-300">Aqui entrará o conteúdo principal do seu painel corporativo.</p>
      </main>
    </div>
  );
}