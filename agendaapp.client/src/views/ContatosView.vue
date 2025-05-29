<template>
  <Toast />

  <Toolbar class="mb-3">
    <template #start>
      <h1 class="m-0">Agenda de Contatos</h1>
    </template>
    <template #end>
      <div class="flex gap-2">
        <Button label="Novo Contato" icon="pi pi-plus" @click="openNovo" />
        <Button v-if="token"
                label="Sair"
                icon="pi pi-sign-out"
                severity="danger"
                @click="logout" />
      </div>
    </template>
  </Toolbar>

  <DataTable :value="contatos"
             dataKey="id"
             stripedRows
             responsiveLayout="scroll"
             class="shadow-2">
    <Column field="nome" header="Nome" class="px-2" />
    <Column field="email" header="Email" class="px-2" />
    <Column field="telefone" header="Telefone" class="px-2" />

    <Column header="Ações" style="width:10rem">
      <template #body="{ data }">
        <div class="flex align-items-center gap-2">
          <Button icon="pi pi-trash"
                  severity="danger"
                  rounded
                  class="me-2"
                  @click="confirmRemove(data)" />
          <Button icon="pi pi-pencil"
                  severity="info"
                  rounded
                  @click="openEdit(data)" />
        </div>
      </template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog"
          :header="editing ? 'Editar Contato' : 'Novo Contato'"
          modal>
    <ContatoForm :contato="editing"
                 @salvo="salvoHandler"
                 @cancel="showDialog = false" />
  </Dialog>
  <Dialog v-model:visible="showRemoveDialog"
          header="Remover Contato?"
          modal
          :closable="false"
          :style="{ width: '350px' }">
    <p>Tem certeza que deseja remover <strong>{{ contatoToRemove?.nome }}</strong>?</p>
    <template #footer>
      <Button label="Cancelar" text @click="showRemoveDialog = false" />
      <Button label="Remover" severity="danger" @click="remove(contatoToRemove)" />
    </template>
  </Dialog>
</template>


<script setup>
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import api from '@/axios';
  import { useToast } from 'primevue/usetoast'

  const router = useRouter();
  const toast = useToast();
  const contatos = ref([]);
  const loading = ref(true);
  const token = localStorage.getItem('token');

  if (!token) {
    router.push('/login')
  }

  const loadContatos = async () => {
    try {
      const { data } = await api.get('/contatos');
      contatos.value = data;
    } catch (err) {
      console.error('Erro ao carregar:', err);
      if (err.response?.status === 401) { router.push('/login'); }
    } finally {
      loading.value = false;
    }
  };

  const addContato = (novo) => {
    contatos.value.push(novo);
    toast.add({ severity: 'success', summary: 'Contato criado', life: 2000 });
  };

  const showDialog = ref(false);
  const editing = ref(null);
  const showRemoveDialog = ref(false);
  const contatoToRemove = ref(null);

  function openNovo() { editing.value = null; showDialog.value = true; }
  function openEdit(c) { editing.value = { ...c }; showDialog.value = true; }

  function salvoHandler(resp) {
    if (editing.value) {
      const i = contatos.value.findIndex(x => x.id === resp.id);
      if (i !== -1) contatos.value[i] = resp;
      toast.add({ severity: 'success', summary: 'Contato atualizado', life: 2000 });
    } else {
      contatos.value.push(resp);
      toast.add({ severity: 'success', summary: 'Contato criado', life: 2000 });
    }
    showDialog.value = false;
  }

  const confirmRemove = (contato) => {
    contatoToRemove.value = contato;
    showRemoveDialog.value = true;
  }

  const remove = async (contato) => {
    try {
      await api.delete(`/contatos/${contato.id}`);
      contatos.value = contatos.value.filter(c => c.id !== contato.id);
      toast.add({ severity: 'warn', summary: 'Contato excluído', life: 2000 });
    } catch (err) {
      console.error('Erro ao excluir:', err);
      if (err.response?.status === 401) { router.push('/login'); }
    } finally {
      showRemoveDialog.value = false;
      contatoToRemove.value = null;
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    delete api.defaults.headers.common.Authorization;
    router.push('/login');
  };

  onMounted(loadContatos);
</script>
