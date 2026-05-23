-- ═══════════════════════════════════════════════════════════════
-- ManadaIA - Schema do Banco de Dados Supabase
-- Sistema de Gestão de Rebanho Bovino
-- ═══════════════════════════════════════════════════════════════

-- Execute este script no SQL Editor do Supabase Dashboard

-- ───────────────────────────────────────────────────────────────
-- 1. TABELA: propriedades
-- ───────────────────────────────────────────────────────────────
CREATE TABLE propriedades (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome            TEXT NOT NULL,
    cidade          TEXT NOT NULL,
    estado          CHAR(2) NOT NULL,
    area_hectares   NUMERIC(10, 2),
    inscricao       TEXT,
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    proprietario_id UUID NOT NULL, -- Referência ao auth.users do Supabase
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ
);

CREATE INDEX idx_propriedades_proprietario ON propriedades(proprietario_id);
CREATE INDEX idx_propriedades_active ON propriedades(is_active);

-- ───────────────────────────────────────────────────────────────
-- 2. TABELA: lotes
-- ───────────────────────────────────────────────────────────────
CREATE TABLE lotes (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome            TEXT NOT NULL,
    descricao       TEXT,
    propriedade_id  UUID NOT NULL REFERENCES propriedades(id) ON DELETE CASCADE,
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ
);

CREATE INDEX idx_lotes_propriedade ON lotes(propriedade_id);
CREATE INDEX idx_lotes_active ON lotes(is_active);

-- ───────────────────────────────────────────────────────────────
-- 3. TABELA: animais
-- ───────────────────────────────────────────────────────────────
CREATE TABLE animais (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    brinco              TEXT NOT NULL UNIQUE,
    nome                TEXT NOT NULL,
    raca                TEXT NOT NULL,
    sexo                INT NOT NULL CHECK (sexo IN (1, 2)), -- 1=Macho, 2=Fêmea
    data_nascimento     DATE NOT NULL,
    peso_atual          NUMERIC(10, 2),
    status              INT NOT NULL DEFAULT 1 CHECK (status IN (1, 2, 3, 4)), -- 1=Ativo, 2=Vendido, 3=Morto, 4=Inativo
    propriedade_id      UUID NOT NULL REFERENCES propriedades(id) ON DELETE CASCADE,
    lote_id             UUID REFERENCES lotes(id) ON DELETE SET NULL,
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ
);

CREATE INDEX idx_animais_brinco ON animais(brinco);
CREATE INDEX idx_animais_propriedade ON animais(propriedade_id);
CREATE INDEX idx_animais_lote ON animais(lote_id);
CREATE INDEX idx_animais_status ON animais(status);

-- ───────────────────────────────────────────────────────────────
-- 4. TABELA: pesagens
-- ───────────────────────────────────────────────────────────────
CREATE TABLE pesagens (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    animal_id       UUID NOT NULL REFERENCES animais(id) ON DELETE CASCADE,
    peso            NUMERIC(10, 2) NOT NULL CHECK (peso > 0),
    data_pesagem    TIMESTAMPTZ NOT NULL,
    observacoes     TEXT,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_pesagens_animal ON pesagens(animal_id);
CREATE INDEX idx_pesagens_data ON pesagens(data_pesagem DESC);

-- ───────────────────────────────────────────────────────────────
-- 5. TABELA: vacinas
-- ───────────────────────────────────────────────────────────────
CREATE TABLE vacinas (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    animal_id           UUID NOT NULL REFERENCES animais(id) ON DELETE CASCADE,
    nome_vacina         TEXT NOT NULL,
    data_aplicacao      TIMESTAMPTZ NOT NULL,
    data_proxima_dose   TIMESTAMPTZ,
    lote                TEXT,
    veterinario         TEXT,
    observacoes         TEXT,
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_vacinas_animal ON vacinas(animal_id);
CREATE INDEX idx_vacinas_proxima_dose ON vacinas(data_proxima_dose);

-- ═══════════════════════════════════════════════════════════════
-- ROW LEVEL SECURITY (RLS)
-- ═══════════════════════════════════════════════════════════════

-- Habilitar RLS em todas as tabelas
ALTER TABLE propriedades ENABLE ROW LEVEL SECURITY;
ALTER TABLE lotes ENABLE ROW LEVEL SECURITY;
ALTER TABLE animais ENABLE ROW LEVEL SECURITY;
ALTER TABLE pesagens ENABLE ROW LEVEL SECURITY;
ALTER TABLE vacinas ENABLE ROW LEVEL SECURITY;

-- ───────────────────────────────────────────────────────────────
-- Policies: PROPRIEDADES
-- ───────────────────────────────────────────────────────────────

-- Usuário pode ver apenas suas propriedades
CREATE POLICY "user_read_own_propriedades"
    ON propriedades FOR SELECT
    TO authenticated
    USING (auth.uid() = proprietario_id);

-- Usuário pode criar suas próprias propriedades
CREATE POLICY "user_create_own_propriedades"
    ON propriedades FOR INSERT
    TO authenticated
    WITH CHECK (auth.uid() = proprietario_id);

-- Usuário pode atualizar suas propriedades
CREATE POLICY "user_update_own_propriedades"
    ON propriedades FOR UPDATE
    TO authenticated
    USING (auth.uid() = proprietario_id);

-- Service role tem acesso total
CREATE POLICY "service_role_all_propriedades"
    ON propriedades FOR ALL
    TO service_role
    USING (true)
    WITH CHECK (true);

-- ───────────────────────────────────────────────────────────────
-- Policies: LOTES
-- ───────────────────────────────────────────────────────────────

CREATE POLICY "user_read_own_lotes"
    ON lotes FOR SELECT
    TO authenticated
    USING (
        EXISTS (
            SELECT 1 FROM propriedades
            WHERE propriedades.id = lotes.propriedade_id
            AND propriedades.proprietario_id = auth.uid()
        )
    );

CREATE POLICY "user_create_own_lotes"
    ON lotes FOR INSERT
    TO authenticated
    WITH CHECK (
        EXISTS (
            SELECT 1 FROM propriedades
            WHERE propriedades.id = lotes.propriedade_id
            AND propriedades.proprietario_id = auth.uid()
        )
    );

CREATE POLICY "service_role_all_lotes"
    ON lotes FOR ALL
    TO service_role
    USING (true)
    WITH CHECK (true);

-- ───────────────────────────────────────────────────────────────
-- Policies: ANIMAIS
-- ───────────────────────────────────────────────────────────────

CREATE POLICY "user_read_own_animais"
    ON animais FOR SELECT
    TO authenticated
    USING (
        EXISTS (
            SELECT 1 FROM propriedades
            WHERE propriedades.id = animais.propriedade_id
            AND propriedades.proprietario_id = auth.uid()
        )
    );

CREATE POLICY "user_create_own_animais"
    ON animais FOR INSERT
    TO authenticated
    WITH CHECK (
        EXISTS (
            SELECT 1 FROM propriedades
            WHERE propriedades.id = animais.propriedade_id
            AND propriedades.proprietario_id = auth.uid()
        )
    );

CREATE POLICY "service_role_all_animais"
    ON animais FOR ALL
    TO service_role
    USING (true)
    WITH CHECK (true);

-- ───────────────────────────────────────────────────────────────
-- Policies: PESAGENS
-- ───────────────────────────────────────────────────────────────

CREATE POLICY "user_read_own_pesagens"
    ON pesagens FOR SELECT
    TO authenticated
    USING (
        EXISTS (
            SELECT 1 FROM animais
            JOIN propriedades ON propriedades.id = animais.propriedade_id
            WHERE animais.id = pesagens.animal_id
            AND propriedades.proprietario_id = auth.uid()
        )
    );

CREATE POLICY "service_role_all_pesagens"
    ON pesagens FOR ALL
    TO service_role
    USING (true)
    WITH CHECK (true);

-- ───────────────────────────────────────────────────────────────
-- Policies: VACINAS
-- ───────────────────────────────────────────────────────────────

CREATE POLICY "user_read_own_vacinas"
    ON vacinas FOR SELECT
    TO authenticated
    USING (
        EXISTS (
            SELECT 1 FROM animais
            JOIN propriedades ON propriedades.id = animais.propriedade_id
            WHERE animais.id = vacinas.animal_id
            AND propriedades.proprietario_id = auth.uid()
        )
    );

CREATE POLICY "service_role_all_vacinas"
    ON vacinas FOR ALL
    TO service_role
    USING (true)
    WITH CHECK (true);

-- ═══════════════════════════════════════════════════════════════
-- FUNÇÕES E TRIGGERS
-- ═══════════════════════════════════════════════════════════════

-- Função para atualizar updated_at automaticamente
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Triggers para updated_at
CREATE TRIGGER set_updated_at_propriedades
    BEFORE UPDATE ON propriedades
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER set_updated_at_lotes
    BEFORE UPDATE ON lotes
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER set_updated_at_animais
    BEFORE UPDATE ON animais
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- ═══════════════════════════════════════════════════════════════
-- VIEWS ÚTEIS
-- ═══════════════════════════════════════════════════════════════

-- View de animais com informações agregadas
CREATE OR REPLACE VIEW view_animais_completo AS
SELECT 
    a.id,
    a.brinco,
    a.nome,
    a.raca,
    CASE a.sexo
        WHEN 1 THEN 'Macho'
        WHEN 2 THEN 'Fêmea'
    END as sexo,
    a.data_nascimento,
    EXTRACT(YEAR FROM AGE(a.data_nascimento)) as idade_anos,
    a.peso_atual,
    CASE a.status
        WHEN 1 THEN 'Ativo'
        WHEN 2 THEN 'Vendido'
        WHEN 3 THEN 'Morto'
        WHEN 4 THEN 'Inativo'
    END as status,
    p.nome as propriedade_nome,
    l.nome as lote_nome,
    a.created_at
FROM animais a
JOIN propriedades p ON p.id = a.propriedade_id
LEFT JOIN lotes l ON l.id = a.lote_id;

-- ═══════════════════════════════════════════════════════════════
-- DADOS DE EXEMPLO (OPCIONAL - Remover em produção)
-- ═══════════════════════════════════════════════════════════════

-- Você pode adicionar dados de exemplo aqui para testes
-- INSERT INTO propriedades (nome, cidade, estado, proprietario_id) VALUES ...

COMMIT;
