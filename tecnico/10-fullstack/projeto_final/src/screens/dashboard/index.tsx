import { useEffect, useState } from "react";
import SidebarComponent from "../../components/Sidebar";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Progress } from "@/components/ui/progress";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Trash2, AlertTriangle, CheckSquare, Play, Bell, Settings } from "lucide-react";
import { tarefaEndpoints } from "@/services/endpoints/tarefas";
import type { Tarefa } from "@/services/endpoints/tarefas";
import customToast from "@/components/CustomToast"; 

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
import { Bar, Doughnut } from "react-chartjs-2";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend
);

interface DashboardScreenProps {
  onLogout: () => Promise<void> | void;
}

export default function DashboardScreen({ onLogout }: DashboardScreenProps) {
  const [tarefas, setTarefas] = useState<Tarefa[]>([]);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    async function carregarDados() {
      try {
        const dados = await tarefaEndpoints.obterTodas();
        setTarefas(dados);
      } catch (error: any) {
        const mensagem = error.response?.data?.errors?.[0] || error.message || "Falha ao atualizar dados do dashboard.";
        customToast({
          title: "Erro de Conexão",
          message: mensagem,
          type: "error",
          onClose: () => {}
        });
      } finally {
        setCarregando(false);
      }
    }
    carregarDados();
  }, []);

  const tarefasEmAberto = tarefas.filter(t => t.situacao === 1).length;
  const emAndamento = tarefas.filter(t => t.situacao === 2).length;
  const concluidas = tarefas.filter(t => t.situacao === 3).length;
  const atrasadas = tarefas.filter(t => t.situacao !== 3 && new Date(t.dataCriacao).getTime() < new Date().getTime() - (5 * 24 * 60 * 60 * 1000)).length;

  const usuariosAgrupados = tarefas.reduce((acc, tarefa) => {
    const email = tarefa.responsavel?.email || "Sem Responsável";
    if (!acc[email]) acc[email] = { total: 0, concluidas: 0 };
    acc[email].total += 1;
    if (tarefa.situacao === 3) acc[email].concluidas += 1;
    return acc;
  }, {} as Record<string, { total: number; concluidas: number }>);

  const dadosResponsaveis = Object.entries(usuariosAgrupados).map(([email, info]) => ({
    nome: email.split('@')[0],
    porcentagem: Math.round((info.concluidas / info.total) * 100) || 0,
    texto: `${info.concluidas}/${info.total} feitas`
  }));

  const totalTarefas = tarefas.length;

  const prioridadeAlta = Math.round((tarefas.filter(t => t.prioridade === 3).length / totalTarefas) * 100) || 0;
  const prioridadeMedia = Math.round((tarefas.filter(t => t.prioridade === 2).length / totalTarefas) * 100) || 0;
  const prioridadeBaixa = Math.round((tarefas.filter(t => t.prioridade === 1).length / totalTarefas) * 100) || 0;

  const donutData = {
    labels: ["Alta/Crítica", "Média", "Baixa"],
    datasets: [
      {
        data: [prioridadeAlta, prioridadeMedia, prioridadeBaixa],
        backgroundColor: ["#ef4444", "#3b82f6", "#10b981"],
        borderWidth: 0,
        cutout: "70%",
      },
    ],
  };

  const donutOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false },
    },
  };

  const nomesTarefas = tarefas.map((t) => t.nome.substring(0, 15) + "...");
  const valoresTarefas = tarefas.map((t) => t.prioridade * 30);
  const listaResponsaveis = tarefas.map((t) => t.responsavel?.email ? t.responsavel.email.split('@')[0] : "Sem dono");

  const barData = {
    labels: nomesTarefas,
    datasets: [
      {
        data: valoresTarefas,
        backgroundColor: tarefas.map((_, index) => index === tarefas.length - 1 ? "#a855f7" : "#1e293b"),
        hoverBackgroundColor: tarefas.map((_, index) => index === tarefas.length - 1 ? "#c084fc" : "#334155"),
        borderRadius: 4,
        barPercentage: 0.6,
      },
    ],
  };

  const barOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false },
      tooltip: {
        backgroundColor: "#131b2e",
        titleColor: "#f1f5f9",
        bodyColor: "#94a3b8",
        borderColor: "#1e293b",
        borderWidth: 1,
        padding: 10,
        displayColors: false,
        callbacks: {
          label: function (context: any) {
            const index = context.dataIndex;
            const valor = context.parsed.y;
            const dono = listaResponsaveis[index];
            return [`Esforço: ${valor}`, `Responsável: ${dono}`];
          },
        },
      },
    },
    scales: {
      x: { display: false }, 
      y: { display: false },
    },
  };

  if (carregando) {
    return <div className="flex h-screen items-center justify-center bg-[#090d16] text-white">Carregando dados reais do banco...</div>;
  }

  return (
    <div className="text-white bg-[#0f172a] min-h-screen flex flex-row font-sans selection:bg-blue-500/30">
      <SidebarComponent onLogout={onLogout} />

      <div className="flex flex-col w-full overflow-y-auto max-h-screen">
        <header className="flex justify-between items-center border-b border-slate-800 bg-[#0f172a] px-8 py-5 sticky top-0 z-10">
          <div>
            <h1 className="text-xl font-bold text-slate-100 font-['Inter']">Dashboard</h1>
          </div>

          <div className="flex items-center gap-4 text-xs text-slate-400">
            <span>Última atualização: agora mesmo</span>

            <Button variant="ghost" size="icon" className="bg-amber-500/10 text-amber-500 hover:bg-amber-500/20 rounded-xl relative">
              <Bell size={16} />
              <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-amber-500 rounded-full"></span>
            </Button>

            <Button variant="ghost" size="icon" className="bg-slate-800 text-slate-300 hover:bg-slate-700 rounded-xl">
              <Settings size={16} />
            </Button>

            <Avatar className="w-8 h-8 rounded-xl shadow-lg shadow-blue-500/10">
              <AvatarFallback className="bg-blue-600 text-white font-bold text-xs rounded-xl">TH</AvatarFallback>
            </Avatar>
          </div>
        </header>

        <main className="flex-1 p-6 space-y-6 bg-[#090d16]">

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <Card className="border-slate-800 bg-[#131b2e] h-32 flex flex-col justify-between hover:border-slate-700 transition-all text-white">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 p-5 pb-0">
                <CardTitle className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">Tarefas em aberto</CardTitle>
                <Trash2 size={16} className="text-amber-600/60" />
              </CardHeader>
              <CardContent className="p-5 pt-0">
                <div className="text-3xl font-bold text-slate-100">{tarefasEmAberto}</div>
                <p className="text-[11px] text-slate-500 mt-1">Aguardando início</p>
              </CardContent>
            </Card>

            <Card className="border-slate-800 bg-[#131b2e] h-32 flex flex-col justify-between hover:border-slate-700 transition-all text-white">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 p-5 pb-0">
                <CardTitle className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">Atrasadas</CardTitle>
                <AlertTriangle size={16} className="text-slate-500" />
              </CardHeader>
              <CardContent className="p-5 pt-0">
                <div className="text-3xl font-bold text-rose-500">{atrasadas}</div>
                <p className="text-[11px] text-slate-500 mt-1">Sinal de atenção</p>
              </CardContent>
            </Card>

            <Card className="border-slate-800 bg-[#131b2e] h-32 flex flex-col justify-between hover:border-slate-700 transition-all text-white">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 p-5 pb-0">
                <CardTitle className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">Concluídas</CardTitle>
                <CheckSquare size={16} className="text-slate-500" />
              </CardHeader>
              <CardContent className="p-5 pt-0">
                <div className="text-3xl font-bold text-emerald-400">{concluidas}</div>
                <p className="text-[11px] text-slate-500 mt-1">Finalizadas no banco</p>
              </CardContent>
            </Card>

            <Card className="border-slate-800 bg-[#131b2e] h-32 flex flex-col justify-between hover:border-slate-700 transition-all text-white">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 p-5 pb-0">
                <CardTitle className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">Em andamento</CardTitle>
                <Play size={16} className="text-slate-500" />
              </CardHeader>
              <CardContent className="p-5 pt-0">
                <div className="text-3xl font-bold text-amber-400">{emAndamento}</div>
                <p className="text-[11px] text-slate-500 mt-1">Sendo executadas</p>
              </CardContent>
            </Card>
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-5 gap-4">
            <Card className="lg:col-span-3 border-slate-800 bg-[#131b2e] text-white flex flex-col justify-between p-5 space-y-0">
              <CardHeader className="p-0 mb-5">
                <CardTitle className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
                  Progresso por Responsável (Banco de Dados)
                </CardTitle>
              </CardHeader>
              <CardContent className="p-0 space-y-4 flex-1 flex flex-col justify-center">
                {dadosResponsaveis.map((resp, index) => (
                  <div key={index} className="space-y-1.5">
                    <div className="flex justify-between text-xs">
                      <span className="text-slate-300 font-medium capitalize">{resp.nome}</span>
                      <span className="text-slate-500">{resp.texto} ({resp.porcentagem}%)</span>
                    </div>
                    <Progress value={resp.porcentagem} className="h-2 bg-slate-800/40" />
                  </div>
                ))}
              </CardContent>
            </Card>

            {/* DONUT */}
            <Card className="lg:col-span-2 border-slate-800 bg-[#131b2e] text-white flex flex-col justify-between p-5 space-y-0">
              <CardHeader className="p-0 mb-2">
                <CardTitle className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
                  Proporção por Prioridade
                </CardTitle>
              </CardHeader>
              <CardContent className="p-0 flex flex-col items-center justify-center flex-1">
                <div className="w-full h-36 relative flex items-center justify-center">
                  <Doughnut data={donutData} options={donutOptions} />
                </div>
                <div className="grid grid-cols-1 gap-x-6 gap-y-1 text-[11px] text-slate-400 mt-2 w-full border-t border-slate-800/60 pt-3">
                  {donutData.labels.map((label, i) => (
                    <div key={label} className="flex justify-between items-center">
                      <div className="flex items-center gap-1.5">
                        <span className="w-2 h-2 rounded-full" style={{ backgroundColor: donutData.datasets[0].backgroundColor[i] }}></span>
                        <span>{label}</span>
                      </div>
                      <span className="font-medium text-slate-300">{donutData.datasets[0].data[i]}%</span>
                    </div>
                  ))}
                </div>
              </CardContent>
            </Card>
          </div>

          <div className="grid grid-cols-1 gap-4">
            <Card className="w-full border-slate-800 bg-[#131b2e] text-white p-5 space-y-0">
              <CardHeader className="p-0 mb-6">
                <CardTitle className="text-xs font-semibold text-slate-400 uppercase tracking-wider">
                  Mapeamento de Esforço por Tarefa Real
                </CardTitle>
              </CardHeader>
              <CardContent className="p-0 h-32 w-full relative">
                <Bar data={barData} options={barOptions} />
              </CardContent>
            </Card>
          </div>
        </main>
      </div>
    </div>
  );
}