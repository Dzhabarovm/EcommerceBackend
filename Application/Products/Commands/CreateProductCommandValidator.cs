using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название товара обязательно")
                .MaximumLength(100).WithMessage("Максимальная длина названия — 100 символов");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Цена должна быть больше 0");

            RuleFor(x => x.ShopId)
                .NotEmpty().WithMessage("Магазин обязателен")
                .MustAsync(BeValidShop).WithMessage("Магазин не существует или доступ запрещен");
        }

        private async Task<bool> BeValidShop(Guid shopId, CancellationToken ct)
        {
            // Реализуйте проверку существования магазина и прав доступа
            // Пример: текущий пользователь — владелец магазина
            return await Task.FromResult(true); // Заглушка
        }
    }
}
