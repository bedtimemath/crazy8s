<?php
/**
 * BedtimeMath Teaser class.
 *
 * @category   Class
 * @package    ElementorBedtimeMath
 * @subpackage WordPress
 * @author     Dug Steen <dug@bedtimemath.org>
 * @copyright  2023 Bedtime Math
 * @license    https://opensource.org/licenses/GPL-3.0 GPL-3.0-only
 * @link       link(https://www.benmarshall.me/build-custom-elementor-widgets/,
 *             Build Custom Elementor Widgets)
 * @since      1.0.0
 * php version 8.1
 */
namespace ElementorBedtimeMath\Widgets;
use Elementor\Widget_Base;
use Elementor\Controls_Manager;

// Security Note: Blocks direct access to the plugin PHP files.
defined( 'ABSPATH' ) || die();

/**
 * BedtimeMath widget class.
 *
 * @since 1.0.0
 */
class BedtimeMath_Teaser extends Widget_Base {
	/**
	 * Class constructor.
	 *
	 * @param array $data Widget data.
	 * @param array $args Widget arguments.
	 */
	public function __construct( $data = array(), $args = null ) {
		parent::__construct( $data, $args );
		wp_register_style( 'bedtimemath-style', plugins_url( '/assets/css/bedtimemath.css', ELEMENTOR_BEDTIMEMATH ), array(), '1.0.0' );
		wp_register_style( 'bedtimemath-teaser-style', plugins_url( '/assets/css/bedtimemath-teaser.css', ELEMENTOR_BEDTIMEMATH ), array(), '1.0.0' );
	}
	/**
	 * Retrieve the widget name.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget name.
	 */
	public function get_name() {
		return 'bedtimemath_teaser';
	}
	/**
	 * Retrieve the widget title.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget title.
	 */
	public function get_title() {
		return __( 'Bedtime Math Teaser', 'elementor-bedtimemath' );
	}
	/**
	 * Retrieve the widget icon.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget icon.
	 */
	public function get_icon() {
		return 'eicon-post-excerpt';
	}
	/**
	 * Retrieve the list of categories the widget belongs to.
	 *
	 * Used to determine where to display the widget in the editor.
	 *
	 * Note that currently Elementor supports only one category.
	 * When multiple categories passed, Elementor uses the first one.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return array Widget categories.
	 */
	public function get_categories() {
		return array( 'bedtimemath' );
	}
	
	/**
	 * Enqueue styles.
	 */
	public function get_style_depends() {
		return array( 'bedtimemath-style', 'bedtimemath-teaser-style' );
	}
	/**
	 * Register the widget controls.
	 *
	 * Adds different input fields to allow the user to change and customize the widget settings.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function _register_controls() {
		
		// SHOW / HIDE CONTROLS
		$this->start_controls_section(
		  'content_showhide_section',
		  [
			'label' => __( 'Show / Hide', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'show_title',
			[
				'type' => \Elementor\Controls_Manager::SWITCHER,
				'label' => esc_html__('Show Title', 'elementor-bedtimemath'),
				'label_on' => esc_html__('Show', 'elementor-bedtimemath'),
				'label_off' => esc_html__('Hide', 'elementor-bedtimemath'),
				'return_value' => 'yes',
				'default' => 'yes'
			]
		);
		$this->add_control(
			'show_date',
			[
				'type' => \Elementor\Controls_Manager::SWITCHER,
				'label' => esc_html__('Show Date', 'elementor-bedtimemath'),
				'label_on' => esc_html__('Show', 'elementor-bedtimemath'),
				'label_off' => esc_html__('Hide', 'elementor-bedtimemath'),
				'return_value' => 'yes',
				'default' => 'yes'
			]
		);
		$this->add_control(
			'show_thumbnail',
			[
				'type' => \Elementor\Controls_Manager::SWITCHER,
				'label' => esc_html__('Show Image', 'elementor-bedtimemath'),
				'label_on' => esc_html__('Show', 'elementor-bedtimemath'),
				'label_off' => esc_html__('Hide', 'elementor-bedtimemath'),
				'return_value' => 'yes',
				'default' => 'yes'
			]
		);
		$this->add_control(
			'show_story',
			[
				'type' => \Elementor\Controls_Manager::SWITCHER,
				'label' => esc_html__('Show Story', 'elementor-bedtimemath'),
				'label_on' => esc_html__('Show', 'elementor-bedtimemath'),
				'label_off' => esc_html__('Hide', 'elementor-bedtimemath'),
				'return_value' => 'yes',
				'default' => 'yes'
			]
		);
		$this->add_control(
			'show_button',
			[
				'type' => \Elementor\Controls_Manager::SWITCHER,
				'label' => esc_html__('Show Button', 'elementor-bedtimemath'),
				'label_on' => esc_html__('Show', 'elementor-bedtimemath'),
				'label_off' => esc_html__('Hide', 'elementor-bedtimemath'),
				'return_value' => 'yes',
				'default' => 'yes'
			]
		);
		
		$this->end_controls_section();

		//$this->add_control( 'hr', [ 'type' => \Elementor\Controls_Manager::DIVIDER ] );
		
		/* IMAGE CONTROLS */
		
		$this->start_controls_section(
		  'content_imagelayout_section',
		  [
			'label' => __( 'Image Layout', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'image_side',
			[
				'type' => \Elementor\Controls_Manager::SWITCHER,
				'label' => esc_html__('Image Side', 'elementor-bedtimemath'),
				'label_on' => esc_html__('Right', 'elementor-bedtimemath'),
				'label_off' => esc_html__('Left', 'elementor-bedtimemath'),
				'return_value' => 'yes',
				'default' => 'no'
			]
		);

		$allSizes = \Elementor\Group_Control_Image_Size::get_all_image_sizes();
		$this->add_group_control(
			\Elementor\Group_Control_Image_Size::get_type(),
			[
				// Usage: `{name}_size` and `{name}_custom_dimension`, in this case `thumbnail_size` and `thumbnail_custom_dimension`.
				'name' => 'thumbnail', 
				'exclude' => $allSizes,
				'default' => 'custom',
			]
		);

		$this->add_control(
			'image_margin',
			[
				'label' => esc_html__('Image Margin', 'elementor-bedtimemath'),
				'type' => \Elementor\Controls_Manager::DIMENSIONS,
				'size_units' => [ 'px', '%', 'em', 'rem', 'custom' ],
				'selectors' => [
					'{{WRAPPER}} .teaser-thumbnail img' => 'margin: {{TOP}}{{UNIT}} {{RIGHT}}{{UNIT}} {{BOTTOM}}{{UNIT}} {{LEFT}}{{UNIT}};',
				]
			]
		);

		$this->end_controls_section();
		
		/* BUTTON CONTROLS */
		
		$this->start_controls_section(
		  'content_button_section',
		  [
			'label' => __( 'Button Content', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'button_text',
			[
				'label' => esc_html__( 'Button Text', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::TEXT,
				'default' => esc_html__( 'Do The Math', 'elementor-bedtimemath' ),
				'placeholder' => esc_html__( 'Type your button text here', 'elementor-bedtimemath' ),
			]
		);

		$this->add_control(
			'button_align',
			[
				'label' => esc_html__( 'Button Alignment', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::CHOOSE,
				'options' => [
				  'left' => [
					'title' => __( 'Left', 'elementor-bedtimemath' ),
					'icon' => 'eicon-text-align-left',
				  ],
				  'center' => [
					'title' => __( 'Center', 'elementor-bedtimemath' ),
					'icon' => 'eicon-text-align-center',
				  ],
				  'right' => [
					'title' => __( 'Right', 'elementor-bedtimemath' ),
					'icon' => 'eicon-text-align-right',
				  ],
				],
				'default' => 'center',
				'toggle' => true
			]
		);

		$this->add_control(
			'button_size',
			[
				'label' => esc_html__( 'Button Size', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::SELECT,
				'options' => [
				  'xs' => esc_html__( 'Extra Small', 'elementor-bedtimemath' ),
				  'sm' => esc_html__( 'Small', 'elementor-bedtimemath' ),
				  'md' => esc_html__( 'Medium', 'elementor-bedtimemath' ),
				  'lg' => esc_html__( 'Large', 'elementor-bedtimemath' ),
				  'xl' => esc_html__( 'Extra Large', 'elementor-bedtimemath' ),
				],
				'default' => 'md'
			]
		);

		$this->add_control(
			'button_id',
			[
				'label' => esc_html__( 'Button ID', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::TEXT,
				'default' => 'TeaserButton',
				'placeholder' => esc_html__( 'Add your button ID here', 'elementor-bedtimemath' ),
				'description' => esc_html__( 'Please make sure the ID is unique and not used elsewhere on the page this form is displayed. This field allows A-z 0-9 & underscore chars without spaces.', 'elementor-bedtimemath' ),
			]
		);

		$this->end_controls_section();
		
		/* TITLE STYLE */

		$this->start_controls_section(
		  'style_title_section',
		  [
			'label' => __( 'Title', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_STYLE,
		  ]
		);

		$this->add_control(
			'title_text_color',
			[
				'label' => esc_html__( 'Text Color', 'textdomain' ),
				'type' => \Elementor\Controls_Manager::COLOR,
				'selectors' => [
					'{{WRAPPER}} .teaser-title' => 'color: {{VALUE}}',
				],
			]
		);

		$this->add_group_control(
			\Elementor\Group_Control_Typography::get_type(),
			[
				'name' => 'title_typography',
				'selector' => '{{WRAPPER}} .teaser-title',
			]
		);

		$this->end_controls_section();
		
		/* DATE STYLE */

		$this->start_controls_section(
		  'style_date_section',
		  [
			'label' => __( 'Date', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_STYLE,
		  ]
		);

		$this->add_control(
			'date_text_color',
			[
				'label' => esc_html__( 'Text Color', 'textdomain' ),
				'type' => \Elementor\Controls_Manager::COLOR,
				'selectors' => [
					'{{WRAPPER}} .teaser-date' => 'color: {{VALUE}}',
				],
			]
		);

		$this->add_group_control(
			\Elementor\Group_Control_Typography::get_type(),
			[
				'name' => 'date_typography',
				'selector' => '{{WRAPPER}} .teaser-date',
			]
		);

		$this->end_controls_section();
		
		/* STORY STYLE */

		$this->start_controls_section(
		  'style_story_section',
		  [
			'label' => __( 'Story', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_STYLE,
		  ]
		);

		$this->add_control(
			'story_text_color',
			[
				'label' => esc_html__( 'Text Color', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::COLOR,
				'selectors' => [
					'{{WRAPPER}} .teaser-story' => 'color: {{VALUE}}',
				],
			]
		);

		$this->add_group_control(
			\Elementor\Group_Control_Typography::get_type(),
			[
				'name' => 'story_typography',
				'selector' => '{{WRAPPER}} .teaser-story',
			]
		);

		$this->end_controls_section();
		
		/* BUTTON STYLE */

		$this->start_controls_section(
		  'style_button_section',
		  [
			'label' => __( 'Button', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_STYLE,
		  ]
		);

		$this->add_control(
			'button_background_color',
			[
				'label' => esc_html__( 'Background Color', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::COLOR,
				'selectors' => [
					'{{WRAPPER}} .teaser-button' => 'background-color: {{VALUE}}',
				],
			]
		);

		$this->add_control(
			'button_text_color',
			[
				'label' => esc_html__( 'Text Color', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::COLOR,
				'selectors' => [
					'{{WRAPPER}} .teaser-button' => 'color: {{VALUE}}',
				],
			]
		);

		$this->add_group_control(
			\Elementor\Group_Control_Typography::get_type(),
			[
				'name' => 'button_typography',
				'selector' => '{{WRAPPER}} .teaser-button',
			]
		);

		$this->end_controls_section();
	}

	/**
	 * Render the widget output on the frontend.
	 *
	 * Written in PHP and used to generate the final HTML.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function render() {

        // get the most recent post
        $recent_posts = wp_get_recent_posts(array(
            'numberposts' => 1, // Number of recent posts thumbnails to display
            'post_status' => 'publish' // Show only the published posts
        ));
        $post = end($recent_posts);
		
		$publish_date = \DateTime::createFromFormat( 'Y-m-d H:i:s',  $post['post_date'] );
        $thumbnail_url = get_the_post_thumbnail_url($post['ID']);
            
		$title = esc_html__($post['post_title'], 'elementor-bedtimemath');
		$context = $post['post_content'];
        $weeOnesPos = stripos($post['post_content'], '<em>Wee ones:');
        if ($weeOnesPos) { 
	        $context = substr($post['post_content'], 0, $weeOnesPos);
		}

		// read the settings
		$settings = $this->get_settings_for_display();
		
		// set up the various HTML blocks
		$thumbnailHtml = '';
		if ( 'yes' === $settings['show_thumbnail'] ) {
			$thumbnailHtml = '<div class="teaser-thumbnail" ';
			if ( $settings['image_side'] === 'yes' ) {
				$thumbnailHtml = $thumbnailHtml . 'style="float:right;" ';
			} else {
				$thumbnailHtml = $thumbnailHtml . 'style="float:left;" ';
			}
			$thumbnailHtml = $thumbnailHtml . '><img src="' . $thumbnail_url . '" ';
			if ( $settings['thumbnail_custom_dimension']['width'] ) {
				$thumbnailHtml = $thumbnailHtml . 'width="' . $settings['thumbnail_custom_dimension']['width'] . '" ';
			}
			if ( $settings['thumbnail_custom_dimension']['height'] ) {
				$thumbnailHtml = $thumbnailHtml . 'height="' . $settings['thumbnail_custom_dimension']['height'] . '" ';
			}
			$thumbnailHtml = $thumbnailHtml . 'alt="' . $title . '" /></div>';
		}
		$titleHtml = '';
		if ( 'yes' === $settings['show_title'] ) {
			$titleHtml = '<h1 class="teaser-title">' . $title . '</h1>';
		}
		$dateHtml = '';
		if ( 'yes' === $settings['show_date'] ) {
			$dateHtml = '<div class="teaser-date">' . $publish_date->format('F j, Y') . '</div>';
		}
		$storyHtml = '';
		if ( 'yes' === $settings['show_story'] ) {
			$storyHtml = '<div class="teaser-story">' . $context . '</div>';
		}
		$buttonHtml = '';
		if ( 'yes' === $settings['show_button'] ) {
			$buttonHtml = '<div class="elementor-element elementor-align-' . 
				$settings['button_align'] . 
				' elementor-widget elementor-widget-button" data-element_type="widget" data-widget_type="button.default">' .
				'<div class="elementor-widget-container">' .
				'<div class="elementor-button-wrapper">' .
				'<a href="/?redirect_to=latest" id="' .  
				$settings['button_id'] . 
				'" class="elementor-button-link elementor-button elementor-size-' .
				$settings['button_size'] . 
				' teaser-button" role="button">' .
				'<span class="elementor-button-content-wrapper">' .
				'<span class="elementor-button-text">' . $settings['button_text'] . '</span>' .
				'</span></a></div></div></div>';
		}

		?>

		<div class="teaser">

		<?php echo $thumbnailHtml; ?>
		<?php echo $titleHtml; ?>
		<?php echo $dateHtml; ?>
		<?php echo $storyHtml; ?>
		<?php echo $buttonHtml; ?>

		</div>

		<?php
	}
	/**
	 * Render the widget output in the editor.
	 *
	 * Written as a Backbone JavaScript template and used to generate the live preview.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function content_template() {
		/* Not needed. See: https://github.com/elementor/elementor/issues/9208 */
	}
}
?>
