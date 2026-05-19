import { Button } from '@/components/Button';

export default function App() {
  return (
    <main className="flex min-h-screen flex-col gap-4 p-8">
      <h1 className="text-2xl font-semibold">Gestão de Tarefas</h1>
      <p className="text-muted-foreground">
        Projeto base do curso. Comece a implementar a partir daqui.
      </p>
      <div>
        <Button type="button">Começar</Button>
      </div>
    </main>
  );
}
