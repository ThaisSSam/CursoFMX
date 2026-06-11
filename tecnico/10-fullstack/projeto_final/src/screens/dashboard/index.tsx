import { useEffect, useState } from "react";
import SidebarComponent from "../../components/Sidebar";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Progress } from "@/components/ui/progress";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import {
  AlertTriangle,
  CheckSquare,
  Play,
  Bell,
  Settings,
  FolderOpen,
} from "lucide-react";
import { tarefaEndpoints } from "@/services/endpoints/tarefas";
import  customToast  from "@/components/CustomToast";
import { useNavigate } from "react-router-dom";

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
  Legend,
);

interface DashboardScreenProps {
  onLogout: () => Promise<void> | void;
}

interface ResponsavelMetrica {
  nome: string;
  totalTarefas: number;
  emAberto: number;
  emAndamento: number;
  concluidas: number;
  atrasadas: number;
  prioridadeAlta: number;
  prioridadeMedia: number;
  prioridadeBaixa: number;
}

export default function DashboardScreen({ onLogout }: DashboardScreenProps) {
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();
  
  const [responsaveisMetricas, setResponsaveisMetricas] = useState<ResponsavelMetrica[]>([]);
  
  const [totaisGlobais, setTotaisGlobais] = useState({
    emAberto: 0,
    emAndamento: 0,
    concluidas: 0,
    atrasadas: 0,
    prioridadeAlta: 0,
    prioridadeMedia: 0,
    prioridadeBaixa: 0,
  });

  async function carregarDados() {
    try {
      setCarregando(true);
      const dadosMetricas: ResponsavelMetrica[] = await tarefaEndpoints.obterMetricasDashboard();
      
      setResponsaveisMetricas(dadosMetricas || []);

      if (dadosMetricas && dadosMetricas.length > 0) {
        const soma = dadosMetricas.reduce(
          (acc, atual) => {
            acc.emAberto += atual.emAberto;
            acc.emAndamento += atual.emAndamento;
            acc.concluidas += atual.concluidas;
            acc.atrasadas += atual.atrasadas;
            acc.prioridadeAlta += atual.prioridadeAlta;
            acc.prioridadeMedia += atual.prioridadeMedia;
            acc.prioridadeBaixa += original_name_resolver(atual);
            return acc;
          },
          { emAberto: 0, emAndamento: 0, concluidas: 0, atrasadas: 0, prioridadeAlta: 0, prioridadeMedia: 0, prioridadeBaixa: 0 }
        );
        
        function original_name_resolver(atual: ResponsavelMetrica) {
            return atual.prioridadeBaixa;
        }

        setTotaisGlobais(soma);
      }
    } catch (error: any) {
      const mensagem =
        error.response?.data?.errors?.[0] ||
        error.message ||
        "Falha ao atualizar dados do dashboard.";

      customToast({
        title: "Erro de Conexão",
        message: mensagem,
        type: "error",
        onClose: () => { },
      });
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    carregarDados();
  }, []);

  const donutData = {
    labels: ["Alta/Crítica", "Média", "Baixa"],
    datasets: [
      {
        data: [totaisGlobais.prioridadeAlta, totaisGlobais.prioridadeMedia, totaisGlobais.prioridadeBaixa],
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

  const barData = {
    labels: responsaveisMetricas.map((r) => r.nome),
    datasets: [
      {
        label: "Total de Tarefas",
        data: responsaveisMetricas.map((r) => r.totalTarefas),
        backgroundColor: "#a855f7",
        borderRadius: 4,
        barPercentage: 0.4,
      },
    ],
  };

  const barOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false },
    },
    scales: {
      x: { grid: { display: false }, ticks: { color: "#94a3b8" } },
      y: { grid: { color: "#1e293b" }, ticks: { color: "#94a3b8" } },
    },
  };

  if (carregando) {
    return (
      <div className="flex h-screen items-center justify-center bg-[#090d16] text-white">
        Carregando dados reais do banco...
      </div>
    );
  }

  return (
    <div className="text-white bg-[#0f172a] min-h-screen flex flex-row font-sans selection:bg-blue-500/30">
      <SidebarComponent
        currentPath="/home"
        onNavigate={(path) => navigate(path)}
        onLogout={onLogout}
      />

      <div className="flex flex-col w-full overflow-y-auto max-h-screen">
        <header className="flex justify-between items-center border-b border-slate-800 bg-[#0f172a] px-8 py-5 sticky top-0 z-10">
          <div>
            <h1 className="text-xl font-bold text-slate-100 font-['Inter']">
              Dashboard
            </h1>
          </div>

          <div className="flex items-center gap-4 text-xs text-slate-400">
            <span>Última atualização: agora mesmo</span>

            <Button
              variant="ghost"
              size="icon"
              className="bg-amber-500/10 text-amber-500 hover:bg-amber-500/20 rounded-xl relative"
            >
              <Bell size={16} />
              <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-amber-500 rounded-full"></span>
            </Button>

            <Button
              variant="ghost"
              size="icon"
              className="bg-slate-800 text-slate-300 hover:bg-slate-700 rounded-xl"
            >
              <Settings size={16} />
            </Button>

            <Avatar className="w-8 h-8 rounded-xl shadow-lg shadow-blue-500/10">
              <AvatarFallback className="bg-blue-600 text-white font-bold text-xs rounded-xl">
                TH
              </AvatarFallback>
            </Avatar>
          </div>
        </header>

        <main className="flex-1 p-6 space-y-6 bg-[#090d16]">
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <Card className="border-slate-800 bg-[#131b2e] h-32 flex flex-col justify-between hover:border-slate-700 transition-all text-white">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 p-5 pb-0">
                <CardTitle className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">
                  Tarefas em aberto
                </CardTitle>
                <FolderOpen size={16} className="text-slate-400" />
              </CardHeader>
              <CardContent className="p-5 pt-0">
                <div className="text-3xl font-bold text-slate-100">
                  {totaisGlobais.emAberto}
                </div>
                <p className="text-[11px] text-slate-500 mt-1">Aguardando início</p>
              </CardContent>
            </Card>

            <Card className="border-slate-800 bg-[#131b2e] h-32 flex flex-col justify-between hover:border-slate-700 transition-all text-white">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 p-5 pb-0">
                <CardTitle className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">
                  Atrasadas
                </CardTitle>
                <AlertTriangle size={16} className="text-slate-400" />
              </CardHeader>
              <CardContent className="p-5 pt-0">
                <div className="text-3xl font-bold text-rose-500">
                  {totaisGlobais.atrasadas}
                </div>
                <p className="text-[11px] text-slate-500 mt-1">Sinal de atenção</p>
              </CardContent>
            </Card>

            <Card className="border-slate-800 bg-[#131b2e] h-32 flex flex-col justify-between hover:border-slate-700 transition-all text-white">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 p-5 pb-0">
                <CardTitle className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">
                  Concluídas
                </CardTitle>
                <CheckSquare size={16} className="text-slate-400" />
              </CardHeader>
              <CardContent className="p-5 pt-0">
                <div className="text-3xl font-bold text-emerald-400">
                  {totaisGlobais.concluidas}
                </div>
                <p className="text-[11px] text-slate-500 mt-1">Finalizadas no banco</p>
              </CardContent>
            </Card>

            <Card className="border-slate-800 bg-[#131b2e] h-32 flex flex-col justify-between hover:border-slate-700 transition-all text-white">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 p-5 pb-0">
                <CardTitle className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">
                  Em andamento
                </CardTitle>
                <Play size={16} className="text-slate-400" />
              </CardHeader>
              <CardContent className="p-5 pt-0">
                <div className="text-3xl font-bold text-amber-400">
                  {totaisGlobais.emAndamento}
                </div>
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
                {responsaveisMetricas.map((resp, index) => {
                  const porcentagemfeitas = Math.round((resp.concluidas / resp.totalTarefas) * 100) || 0;

                  return (
                    <div key={index} className="space-y-1.5">
                      <div className="flex justify-between text-xs">
                        <span className="text-slate-300 font-medium capitalize">
                          {resp.nome}
                        </span>
                        <span className="text-slate-500">
                          {resp.concluidas}/{resp.totalTarefas} feitas ({porcentagemfeitas}%)
                        </span>
                      </div>
                      <Progress
                        value={porcentagemfeitas}
                        className="h-2 bg-slate-800/40"
                      />
                    </div>
                  );
                })}
              </CardContent>
            </Card>

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
                    <div
                      key={label}
                      className="flex justify-between items-center"
                    >
                      <div className="flex items-center gap-1.5">
                        <span
                          className="w-2 h-2 rounded-full"
                          style={{
                            backgroundColor: donutData.datasets[0].backgroundColor[i],
                          }}
                        ></span>
                        <span>{label}</span>
                      </div>
                      <span className="font-medium text-slate-300">
                        {donutData.datasets[0].data[i]}
                      </span>
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
                  Volume de Tarefas por Responsável
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