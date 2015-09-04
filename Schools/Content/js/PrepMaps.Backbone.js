$(function () {

    var CategoryItem = Backbone.Model.extend({
        defaults: {
            Name: '',
			Description: '',
			Range: '',
            Color: '',
			Enabled: true,
			Disabled: function() {
				return this.Enabled === false;
			}
        }
    });

    window.CategoryList = Backbone.Collection.extend({
        model: CategoryItem
    });

    var CategoryItemView = Backbone.View.extend({
        tagName: 'li', // name of (orphan) root tag in this.el
		className: 'clear',
		template: $('#CategoryItemTemplate').html(),
		
		events: {
            'click .icon': 'toggleCategory'
        },
		
        initialize: function () {
            _.bindAll(this, 'render'); // every function that uses 'this' as the current object should be in here
        },
		
        render: function () {
			var modelData = this.model.toJSON();
			var itemHtml = Mustache.to_html(this.template, modelData);
			$(this.el).html(itemHtml);
            return this; // for chainable calls, like .render().el
        },
		
		toggleCategory: function() {
			if(this.model.get('Enabled') === true) {
				PrepMaps.Core.HideCategory(this.model.get('Name'));
				this.model.set({Enabled: false});
			}
			else {
				PrepMaps.Core.ShowCategory(this.model.get('Name'));
				this.model.set({Enabled: true});
			}
			this.render();
		}
    });

    window.CategoryListView = Backbone.View.extend({
        el: $('body'), // el attaches to existing element
        events: {
            'click #findme': 'findCurrentLocation'
        },
        initialize: function () {
            _.bindAll(this, 'render', 'findCurrentLocation', 'appendCategoryItem'); // every function that uses 'this' as the current object should be in here

			if(!this.collection){
				this.collection = new CategoryList();
			}
            this.collection.bind('add', this.appendCategoryItem); // collection event binder

            this.counter = 0;
            this.render();
        },
        render: function () {
            _(this.collection.models).each(function (item) { // in case collection is not empty
                this.appendCategoryItem(item);
            }, this);
        },
        findCurrentLocation: function () {
            PrepMaps.Core.SetMapToCurrentLocation();
        },
            
        appendCategoryItem: function (categoryItem) {
            var categoryItemView = new CategoryItemView({
                model: categoryItem
            });
            $('ul#category-container', this.el).append(categoryItemView.render().el);
        }
    });

});