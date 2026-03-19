 -- Module Core — Nền Tảng Chung

CREATE TABLE companies (
    id              BIGSERIAL PRIMARY KEY,
    code            VARCHAR(20) UNIQUE NOT NULL,
    name            VARCHAR(255) NOT NULL,
    tax_code        VARCHAR(20),
    address         TEXT,
    phone           VARCHAR(20),
    email           VARCHAR(100),
    currency_code   VARCHAR(3) DEFAULT 'VND',
    fiscal_year_start INT DEFAULT 1,  -- tháng bắt đầu năm tài chính
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE branches (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    code            VARCHAR(20) NOT NULL,
    name            VARCHAR(255) NOT NULL,
    address         TEXT,
    phone           VARCHAR(20),
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, code)
);
-- Bảng departments — Phòng ban
CREATE TABLE departments (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    branch_id       BIGINT REFERENCES branches(id),
    code            VARCHAR(20) NOT NULL,
    name            VARCHAR(255) NOT NULL,
    parent_id       BIGINT REFERENCES departments(id),  -- phòng ban cha
    manager_id      BIGINT,  -- FK đến employees, thêm sau
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, code)
);
-- Bảng currencies & exchange_rates
CREATE TABLE currencies (
    code            VARCHAR(3) PRIMARY KEY,   -- VND, USD, EUR
    name            VARCHAR(100) NOT NULL,
    symbol          VARCHAR(10),
    decimal_places  INT DEFAULT 0,
    is_active       BOOLEAN DEFAULT TRUE
);

CREATE TABLE exchange_rates (
    id              BIGSERIAL PRIMARY KEY,
    from_currency   VARCHAR(3) REFERENCES currencies(code),
    to_currency     VARCHAR(3) REFERENCES currencies(code),
    rate            DECIMAL(18,6) NOT NULL,
    effective_date  DATE NOT NULL,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(from_currency, to_currency, effective_date)
);


 -- 3. Module HRM — Nhân Sự
-- Bảng employees — Nhân viên
CREATE TABLE employees (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    department_id   BIGINT REFERENCES departments(id),
    employee_code   VARCHAR(20) NOT NULL,
    first_name      VARCHAR(100) NOT NULL,
    last_name       VARCHAR(100) NOT NULL,
    full_name       VARCHAR(255) GENERATED ALWAYS AS
                    (last_name || ' ' || first_name) STORED,
    gender          VARCHAR(10) CHECK (gender IN ('male','female','other')),
    date_of_birth   DATE,
    id_number       VARCHAR(20),           -- CCCD/CMND
    phone           VARCHAR(20),
    email           VARCHAR(100),
    address         TEXT,
    hire_date       DATE NOT NULL,
    termination_date DATE,
    position_id     BIGINT REFERENCES positions(id),
    manager_id      BIGINT REFERENCES employees(id),
    employment_type VARCHAR(20) DEFAULT 'full_time'
                    CHECK (employment_type IN
                    ('full_time','part_time','contract','intern')),
    base_salary     DECIMAL(18,2) DEFAULT 0,
    bank_account    VARCHAR(30),
    bank_name       VARCHAR(100),
    tax_code        VARCHAR(20),
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, employee_code)
);

CREATE TABLE positions (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    code            VARCHAR(20) NOT NULL,
    name            VARCHAR(255) NOT NULL,
    level           INT DEFAULT 1,
    is_active       BOOLEAN DEFAULT TRUE,
    UNIQUE(company_id, code)
);
--  Bảng attendance — Chấm công
CREATE TABLE attendance (
    id              BIGSERIAL PRIMARY KEY,
    employee_id     BIGINT REFERENCES employees(id),
    work_date       DATE NOT NULL,
    check_in        TIMESTAMP,
    check_out       TIMESTAMP,
    worked_hours    DECIMAL(5,2),
    overtime_hours  DECIMAL(5,2) DEFAULT 0,
    status          VARCHAR(20) DEFAULT 'present'
                    CHECK (status IN
                    ('present','absent','leave','holiday','half_day')),
    note            TEXT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(employee_id, work_date)
);
-- Bảng payroll — Bảng lương
CREATE TABLE payroll (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    employee_id     BIGINT REFERENCES employees(id),
    period_month    INT NOT NULL,          -- tháng
    period_year     INT NOT NULL,          -- năm
    working_days    DECIMAL(5,2),
    base_salary     DECIMAL(18,2),
    allowances      DECIMAL(18,2) DEFAULT 0,
    overtime_pay    DECIMAL(18,2) DEFAULT 0,
    bonuses         DECIMAL(18,2) DEFAULT 0,
    gross_salary    DECIMAL(18,2),
    social_insurance    DECIMAL(18,2) DEFAULT 0,
    health_insurance    DECIMAL(18,2) DEFAULT 0,
    unemployment_insurance DECIMAL(18,2) DEFAULT 0,
    personal_income_tax DECIMAL(18,2) DEFAULT 0,
    other_deductions    DECIMAL(18,2) DEFAULT 0,
    net_salary      DECIMAL(18,2),
    status          VARCHAR(20) DEFAULT 'draft'
                    CHECK (status IN ('draft','confirmed','paid')),
    paid_date       DATE,
    created_by      BIGINT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(employee_id, period_month, period_year)
);



-- 4. Module Inventory — Sản Phẩm & Kho
-- Bảng product_categories & products
CREATE TABLE product_categories (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    code            VARCHAR(20) NOT NULL,
    name            VARCHAR(255) NOT NULL,
    parent_id       BIGINT REFERENCES product_categories(id),
    is_active       BOOLEAN DEFAULT TRUE,
    UNIQUE(company_id, code)
);

CREATE TABLE units_of_measure (
    id              BIGSERIAL PRIMARY KEY,
    code            VARCHAR(10) NOT NULL UNIQUE,  -- cái, kg, m, hộp
    name            VARCHAR(50) NOT NULL
);

CREATE TABLE products (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    category_id     BIGINT REFERENCES product_categories(id),
    sku             VARCHAR(50) NOT NULL,          -- mã sản phẩm
    barcode         VARCHAR(50),
    name            VARCHAR(255) NOT NULL,
    description     TEXT,
    uom_id          BIGINT REFERENCES units_of_measure(id),
    product_type    VARCHAR(20) DEFAULT 'goods'
                    CHECK (product_type IN
                    ('goods','service','raw_material','consumable')),
    cost_price      DECIMAL(18,4) DEFAULT 0,
    selling_price   DECIMAL(18,4) DEFAULT 0,
    min_stock       DECIMAL(18,4) DEFAULT 0,       -- tồn kho tối thiểu
    max_stock       DECIMAL(18,4) DEFAULT 0,
    weight          DECIMAL(10,3),
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, sku)
);
-- Bảng warehouses & stock_moves
CREATE TABLE warehouses (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    branch_id       BIGINT REFERENCES branches(id),
    code            VARCHAR(20) NOT NULL,
    name            VARCHAR(255) NOT NULL,
    address         TEXT,
    is_active       BOOLEAN DEFAULT TRUE,
    UNIQUE(company_id, code)
);

CREATE TABLE warehouse_locations (
    id              BIGSERIAL PRIMARY KEY,
    warehouse_id    BIGINT REFERENCES warehouses(id),
    code            VARCHAR(30) NOT NULL,          -- A1-01-01 (khu-kệ-tầng)
    name            VARCHAR(100),
    is_active       BOOLEAN DEFAULT TRUE,
    UNIQUE(warehouse_id, code)
);

-- Tồn kho hiện tại (materialized view hoặc bảng tổng hợp)
CREATE TABLE stock_on_hand (
    id              BIGSERIAL PRIMARY KEY,
    warehouse_id    BIGINT REFERENCES warehouses(id),
    location_id     BIGINT REFERENCES warehouse_locations(id),
    product_id      BIGINT REFERENCES products(id),
    quantity        DECIMAL(18,4) DEFAULT 0,
    reserved_qty    DECIMAL(18,4) DEFAULT 0,       -- đã giữ cho đơn hàng
    available_qty   DECIMAL(18,4) GENERATED ALWAYS AS
                    (quantity - reserved_qty) STORED,
    last_updated    TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(warehouse_id, location_id, product_id)
);

-- Lịch sử xuất nhập kho
CREATE TABLE stock_moves (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    move_type       VARCHAR(20) NOT NULL
                    CHECK (move_type IN
                    ('in','out','transfer','adjustment')),
    reference_type  VARCHAR(30),    -- 'purchase_order','sale_order','manual'
    reference_id    BIGINT,         -- ID của đơn hàng liên quan
    product_id      BIGINT REFERENCES products(id),
    from_warehouse_id BIGINT REFERENCES warehouses(id),
    to_warehouse_id   BIGINT REFERENCES warehouses(id),
    quantity        DECIMAL(18,4) NOT NULL,
    unit_cost       DECIMAL(18,4),
    move_date       TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    note            TEXT,
    status          VARCHAR(20) DEFAULT 'draft'
                    CHECK (status IN ('draft','confirmed','done','cancelled')),
    created_by      BIGINT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


-- 5. Module Purchasing — Mua Hàng
-- Nhà cung cấp
CREATE TABLE suppliers (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    code            VARCHAR(20) NOT NULL,
    name            VARCHAR(255) NOT NULL,
    tax_code        VARCHAR(20),
    contact_person  VARCHAR(100),
    phone           VARCHAR(20),
    email           VARCHAR(100),
    address         TEXT,
    payment_term_days INT DEFAULT 30,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, code)
);

-- Đơn mua hàng
CREATE TABLE purchase_orders (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    po_number       VARCHAR(30) NOT NULL,
    supplier_id     BIGINT REFERENCES suppliers(id),
    warehouse_id    BIGINT REFERENCES warehouses(id),
    order_date      DATE NOT NULL DEFAULT CURRENT_DATE,
    expected_date   DATE,
    currency_code   VARCHAR(3) REFERENCES currencies(code),
    exchange_rate   DECIMAL(18,6) DEFAULT 1,
    subtotal        DECIMAL(18,2) DEFAULT 0,
    tax_amount      DECIMAL(18,2) DEFAULT 0,
    discount_amount DECIMAL(18,2) DEFAULT 0,
    total_amount    DECIMAL(18,2) DEFAULT 0,
    status          VARCHAR(20) DEFAULT 'draft'
                    CHECK (status IN
                    ('draft','confirmed','receiving','done','cancelled')),
    note            TEXT,
    created_by      BIGINT,
    approved_by     BIGINT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, po_number)
);

CREATE TABLE purchase_order_lines (
    id              BIGSERIAL PRIMARY KEY,
    po_id           BIGINT REFERENCES purchase_orders(id) ON DELETE CASCADE,
    line_number     INT NOT NULL,
    product_id      BIGINT REFERENCES products(id),
    description     VARCHAR(255),
    quantity        DECIMAL(18,4) NOT NULL,
    received_qty    DECIMAL(18,4) DEFAULT 0,
    unit_price      DECIMAL(18,4) NOT NULL,
    tax_rate        DECIMAL(5,2) DEFAULT 0,        -- % thuế
    discount_pct    DECIMAL(5,2) DEFAULT 0,
    line_total      DECIMAL(18,2),
    UNIQUE(po_id, line_number)
);



-- 6. Module Sales — Bán Hàng
-- Khách hàng
CREATE TABLE customers (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    code            VARCHAR(20) NOT NULL,
    name            VARCHAR(255) NOT NULL,
    customer_type   VARCHAR(20) DEFAULT 'company'
                    CHECK (customer_type IN ('company','individual')),
    tax_code        VARCHAR(20),
    contact_person  VARCHAR(100),
    phone           VARCHAR(20),
    email           VARCHAR(100),
    address         TEXT,
    credit_limit    DECIMAL(18,2) DEFAULT 0,
    payment_term_days INT DEFAULT 30,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, code)
);

-- Đơn bán hàng
CREATE TABLE sale_orders (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    so_number       VARCHAR(30) NOT NULL,
    customer_id     BIGINT REFERENCES customers(id),
    warehouse_id    BIGINT REFERENCES warehouses(id),
    order_date      DATE NOT NULL DEFAULT CURRENT_DATE,
    delivery_date   DATE,
    currency_code   VARCHAR(3) REFERENCES currencies(code),
    exchange_rate   DECIMAL(18,6) DEFAULT 1,
    subtotal        DECIMAL(18,2) DEFAULT 0,
    tax_amount      DECIMAL(18,2) DEFAULT 0,
    discount_amount DECIMAL(18,2) DEFAULT 0,
    total_amount    DECIMAL(18,2) DEFAULT 0,
    status          VARCHAR(20) DEFAULT 'draft'
                    CHECK (status IN
                    ('draft','confirmed','delivering','done','cancelled')),
    note            TEXT,
    salesperson_id  BIGINT REFERENCES employees(id),
    created_by      BIGINT,
    approved_by     BIGINT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, so_number)
);

CREATE TABLE sale_order_lines (
    id              BIGSERIAL PRIMARY KEY,
    so_id           BIGINT REFERENCES sale_orders(id) ON DELETE CASCADE,
    line_number     INT NOT NULL,
    product_id      BIGINT REFERENCES products(id),
    description     VARCHAR(255),
    quantity        DECIMAL(18,4) NOT NULL,
    delivered_qty   DECIMAL(18,4) DEFAULT 0,
    unit_price      DECIMAL(18,4) NOT NULL,
    tax_rate        DECIMAL(5,2) DEFAULT 0,
    discount_pct    DECIMAL(5,2) DEFAULT 0,
    line_total      DECIMAL(18,2),
    UNIQUE(so_id, line_number)
);


-- 7. Module Accounting — Kế Toán
-- Hệ thống tài khoản kế toán
CREATE TABLE chart_of_accounts (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    account_code    VARCHAR(20) NOT NULL,       -- 111, 131, 331, 511...
    account_name    VARCHAR(255) NOT NULL,
    account_type    VARCHAR(30) NOT NULL
                    CHECK (account_type IN
                    ('asset','liability','equity','revenue',
                     'expense','cost_of_goods')),
    parent_id       BIGINT REFERENCES chart_of_accounts(id),
    is_detail       BOOLEAN DEFAULT TRUE,       -- tài khoản chi tiết
    is_active       BOOLEAN DEFAULT TRUE,
    UNIQUE(company_id, account_code)
);

-- Bút toán kế toán (Journal Entry)
CREATE TABLE journal_entries (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    entry_number    VARCHAR(30) NOT NULL,
    entry_date      DATE NOT NULL,
    reference_type  VARCHAR(30),     -- 'sale_order','purchase_order','manual'
    reference_id    BIGINT,
    description     TEXT,
    currency_code   VARCHAR(3) REFERENCES currencies(code),
    exchange_rate   DECIMAL(18,6) DEFAULT 1,
    status          VARCHAR(20) DEFAULT 'draft'
                    CHECK (status IN ('draft','posted','cancelled')),
    posted_date     DATE,
    created_by      BIGINT,
    approved_by     BIGINT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, entry_number)
);

-- Chi tiết bút toán (Nợ / Có)
CREATE TABLE journal_entry_lines (
    id              BIGSERIAL PRIMARY KEY,
    entry_id        BIGINT REFERENCES journal_entries(id) ON DELETE CASCADE,
    line_number     INT NOT NULL,
    account_id      BIGINT REFERENCES chart_of_accounts(id),
    description     VARCHAR(255),
    debit_amount    DECIMAL(18,2) DEFAULT 0,    -- Nợ
    credit_amount   DECIMAL(18,2) DEFAULT 0,    -- Có
    partner_type    VARCHAR(20),                 -- 'customer','supplier'
    partner_id      BIGINT,                      -- ID khách hàng/NCC
    UNIQUE(entry_id, line_number)
);

-- Hóa đơn (Invoice) — liên kết Sale/Purchase với Kế toán
CREATE TABLE invoices (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    invoice_number  VARCHAR(30) NOT NULL,
    invoice_type    VARCHAR(20) NOT NULL
                    CHECK (invoice_type IN ('sale','purchase')),
    partner_type    VARCHAR(20) NOT NULL
                    CHECK (partner_type IN ('customer','supplier')),
    partner_id      BIGINT NOT NULL,
    reference_type  VARCHAR(30),
    reference_id    BIGINT,
    invoice_date    DATE NOT NULL,
    due_date        DATE,
    currency_code   VARCHAR(3) REFERENCES currencies(code),
    subtotal        DECIMAL(18,2) DEFAULT 0,
    tax_amount      DECIMAL(18,2) DEFAULT 0,
    total_amount    DECIMAL(18,2) DEFAULT 0,
    paid_amount     DECIMAL(18,2) DEFAULT 0,
    balance_due     DECIMAL(18,2) GENERATED ALWAYS AS
                    (total_amount - paid_amount) STORED,
    status          VARCHAR(20) DEFAULT 'draft'
                    CHECK (status IN
                    ('draft','confirmed','partial_paid','paid','cancelled')),
    journal_entry_id BIGINT REFERENCES journal_entries(id),
    created_by      BIGINT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, invoice_number)
);

-- Phiếu thu / chi (Payment)
CREATE TABLE payments (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    payment_number  VARCHAR(30) NOT NULL,
    payment_type    VARCHAR(20) NOT NULL
                    CHECK (payment_type IN ('inbound','outbound')),
    payment_method  VARCHAR(20) DEFAULT 'bank_transfer'
                    CHECK (payment_method IN
                    ('cash','bank_transfer','check','other')),
    partner_type    VARCHAR(20),
    partner_id      BIGINT,
    invoice_id      BIGINT REFERENCES invoices(id),
    amount          DECIMAL(18,2) NOT NULL,
    currency_code   VARCHAR(3) REFERENCES currencies(code),
    payment_date    DATE NOT NULL,
    bank_account    VARCHAR(50),
    description     TEXT,
    status          VARCHAR(20) DEFAULT 'draft'
                    CHECK (status IN ('draft','confirmed','cancelled')),
    journal_entry_id BIGINT REFERENCES journal_entries(id),
    created_by      BIGINT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, payment_number)
);


-- 8. Module Manufacturing — Sản Xuất (Cơ Bản)
-- Định mức nguyên vật liệu (Bill of Materials)
CREATE TABLE bom (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    product_id      BIGINT REFERENCES products(id),  -- thành phẩm
    bom_code        VARCHAR(30) NOT NULL,
    name            VARCHAR(255),
    quantity         DECIMAL(18,4) DEFAULT 1,         -- số lượng thành phẩm
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, bom_code)
);

CREATE TABLE bom_lines (
    id              BIGSERIAL PRIMARY KEY,
    bom_id          BIGINT REFERENCES bom(id) ON DELETE CASCADE,
    material_id     BIGINT REFERENCES products(id),   -- nguyên vật liệu
    quantity        DECIMAL(18,4) NOT NULL,            -- số lượng cần
    uom_id          BIGINT REFERENCES units_of_measure(id),
    waste_pct       DECIMAL(5,2) DEFAULT 0             -- % hao hụt
);

-- Lệnh sản xuất
CREATE TABLE production_orders (
    id              BIGSERIAL PRIMARY KEY,
    company_id      BIGINT REFERENCES companies(id),
    order_number    VARCHAR(30) NOT NULL,
    bom_id          BIGINT REFERENCES bom(id),
    product_id      BIGINT REFERENCES products(id),
    planned_qty     DECIMAL(18,4) NOT NULL,
    produced_qty    DECIMAL(18,4) DEFAULT 0,
    warehouse_id    BIGINT REFERENCES warehouses(id),
    planned_start   DATE,
    planned_end     DATE,
    actual_start    DATE,
    actual_end      DATE,
    status          VARCHAR(20) DEFAULT 'draft'
                    CHECK (status IN
                    ('draft','confirmed','in_progress','done','cancelled')),
    created_by      BIGINT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, order_number)
);


-- 9. Module System — Phân Quyền & Audit
-- Người dùng hệ thống
CREATE TABLE users (
    id              BIGSERIAL PRIMARY KEY,
    username        VARCHAR(50) UNIQUE NOT NULL,
    password_hash   VARCHAR(255) NOT NULL,
    employee_id     BIGINT REFERENCES employees(id),
    email           VARCHAR(100),
    is_admin        BOOLEAN DEFAULT FALSE,
    is_active       BOOLEAN DEFAULT TRUE,
    last_login      TIMESTAMP,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Vai trò & quyền
CREATE TABLE roles (
    id              BIGSERIAL PRIMARY KEY,
    code            VARCHAR(30) UNIQUE NOT NULL,
    name            VARCHAR(100) NOT NULL,
    description     TEXT
);

CREATE TABLE permissions (
    id              BIGSERIAL PRIMARY KEY,
    module          VARCHAR(50) NOT NULL,       -- 'sales','inventory',...
    action          VARCHAR(20) NOT NULL,       -- 'create','read','update','delete'
    resource        VARCHAR(50) NOT NULL,       -- 'sale_orders','products',...
    UNIQUE(module, action, resource)
);

CREATE TABLE role_permissions (
    role_id         BIGINT REFERENCES roles(id) ON DELETE CASCADE,
    permission_id   BIGINT REFERENCES permissions(id) ON DELETE CASCADE,
    PRIMARY KEY (role_id, permission_id)
);

CREATE TABLE user_roles (
    user_id         BIGINT REFERENCES users(id) ON DELETE CASCADE,
    role_id         BIGINT REFERENCES roles(id) ON DELETE CASCADE,
    company_id      BIGINT REFERENCES companies(id),
    PRIMARY KEY (user_id, role_id, company_id)
);

-- Nhật ký hoạt động
CREATE TABLE audit_logs (
    id              BIGSERIAL PRIMARY KEY,
    user_id         BIGINT REFERENCES users(id),
    action          VARCHAR(20) NOT NULL,       -- INSERT, UPDATE, DELETE
    table_name      VARCHAR(50) NOT NULL,
    record_id       BIGINT,
    old_values      JSONB,                      -- giá trị cũ
    new_values      JSONB,                      -- giá trị mới
    ip_address      INET,
    user_agent      TEXT,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tạo index cho audit_logs (query thường xuyên)
CREATE INDEX idx_audit_logs_user ON audit_logs(user_id);
CREATE INDEX idx_audit_logs_table ON audit_logs(table_name, record_id);
CREATE INDEX idx_audit_logs_date ON audit_logs(created_at);