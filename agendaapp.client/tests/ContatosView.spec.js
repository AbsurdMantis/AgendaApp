import { mount, flushPromises } from '@vue/test-utils'
import { describe, it, expect, vi } from 'vitest'

vi.mock('@/axios', () => ({
  default: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
    delete: vi.fn()
  }
}));

import api from '@/axios'
import ContatosView from '@/views/ContatosView.vue'

describe('ContatosView.vue', () => {
  it('lista contatos após montagem', async () => {
    localStorage.setItem('token', 'dummy-token');

    api.get.mockResolvedValue({
      data: [{ id: 1, nome: 'Astartes Warhammer', email: 'astartes@hotmail.com', telefone: '1893291531' }]
    });

    const wrapper = mount(ContatosView, {
      global: { stubs: { ContatoForm: true } }
    });

    await flushPromises();

    expect(api.get).toHaveBeenCalledWith('/contatos');
    expect(wrapper.text()).toContain('Astartes Warhammer');
    expect(wrapper.text()).toContain('astartes@hotmail.com');
    expect(wrapper.text()).toContain('1893291531');
  })
})
